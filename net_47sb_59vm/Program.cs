﻿using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace net_47sb_59vm
{
    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public string http_method;
        public string http_url;
        public string http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        public static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }


        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public void process()
        {
            inputStream = new BufferedStream(socket.GetStream());
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                    handleGETRequest();
                else if (http_method.Equals("POST"))
                    handlePOSTRequest();
            }
            catch (Exception e)
            {
                Logger.Log("Exception: " + e.ToString());
                writeFailure();
            }
            try
            {
                outputStream.Flush();
            }
            catch { }
            inputStream = null; outputStream = null;
            socket.Close();
        }

        public void parseRequest()
        {
            string request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
                throw new Exception("invalid http request line");
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            Logger.Log("starting: " + request);
        }

        public void readHeaders()
        {
            string line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    Logger.Log("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                    throw new Exception("invalid http header line: " + line);
                string name = line.Substring(0, separator);
                int pos = separator + 1;

                while ((pos < line.Length) && (line[pos] == ' '))
                    pos++;

                string value = line.Substring(pos, line.Length - pos);
                Logger.Log(string.Format("header: {0}:{1}", name, value));
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest()
        {
            Logger.Log("GET");
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest()
        {
            Logger.Log("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        string.Format("POST Content-Length({0}) too big for this server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    Logger.Log("starting Read, to_read={0}", to_read);

                    int numread = inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    Logger.Log("read finished, numread={0}", numread);
                    if (numread == 0)
                    {
                        if (to_read == 0)
                            break;
                        else
                            throw new Exception("client disconnected during post");
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Logger.Log("get post data end");
            Logger.Log("POST");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        public void writeSuccess(string content_type = "text/html")
        {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Language: cs");
            outputStream.WriteLine("Content-Type: " + content_type + string.Format("; charset={0}", Program.cfg.charset));
            outputStream.WriteLine("Connection: close");

            outputStream.WriteLine("");
        }

        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");

            outputStream.WriteLine("");
        }
    }

    public class HttpServer
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;

        public HttpServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            listener = new TcpListener(port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public void handleGETRequest(HttpProcessor p)
        {
            Logger.Log(string.Format("request: {0}", p.http_url));
            if (Interaction.Interaction.Hooks(p))
                return;
            if (Interaction.Interaction.TryName(p.http_url, p))
                return;
            if (Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))) && !File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))))
            {
                p.http_url += (p.http_url == "/" ? "" : "/");
                Logger.Log(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url));
                string[] files = Directory.GetFiles(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), (p.http_url.Length == 1 ? "" : p.http_url.Substring(1))));
                bool flag = false;
                foreach (string filename in files)
                {
                    if (Path.GetFileName(filename).Contains("index"))
                    {
                        Logger.Log(filename);
                        if (!Interaction.Interaction.TryExtension(Path.GetExtension(filename), p, filename))
                        {
                            p.writeSuccess("text/plain");
                            p.outputStream.Write(File.ReadAllText(filename));
                            p.outputStream.Flush();
                            return;
                        }
                        else
                            flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<a href=\"../\">../</a><br>");
                    foreach (string file in files)
                        sb.AppendLine(string.Format("<a href=\"{0}\">{0}</a><br>", file.Replace(Path.Combine(Environment.CurrentDirectory, "server"), "")));
                    p.outputStream.WriteLine(sb.ToString());
                    p.writeSuccess();
                    p.outputStream.Flush();
                    return;
                }
                return;
            }

            //if everything else fails, try 404
            else if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))) && !File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))))
            {
                handle404(p);
                return;
            }
            if (!Interaction.Interaction.TryExtension(Path.GetExtension(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Substring(1))), p))
            {
                p.writeSuccess("text/plain");
                p.outputStream.Write(File.ReadAllText(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Substring(1))));
                p.outputStream.Flush();
            }
            Logger.Save();
        }
        public void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            if (Interaction.Interaction.POST(p, inputData))
                return;
        }

        public static void handle404(HttpProcessor p)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "server/404.html")))
            {
                string fs = File.ReadAllText(Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), "404.html"));
                p.writeSuccess();
                p.outputStream.WriteLine(fs);
                p.outputStream.Flush();
            }
            else
            {
                p.writeSuccess();
                p.outputStream.WriteLine("<h1>404 404 - 404 Not found</h1>".WriteHTMLStub("<title>404</title>"));
                p.outputStream.Flush();
            }
        }
    }
    public class Program
    {
        public static bool Verbose = false;
        public static Configs.Main cfg = new Configs.Main();
        public static Configs.Console consolecfg = new Configs.Console(true);
        public static event EventHandler<HandledEventArgs> Exit = delegate { };
        public volatile static bool PendingLogSave = false;
        public volatile static Thread LoggerThread;
        public static Thread ConsoleThread;
        public static Thread Listener;
        public static Thread ControlThread;
        public static int Main(string[] args)
        {
            PerformFileCheck();
            cfg = new Configs.Main();
            Console.Title = "net_47sb_59vm";
            HttpServer httpServer;
            SubdomainService.Initialize();
            ComponentLoader.Initialize();
            if (args.Length > 0)
            {
                httpServer = new HttpServer(Convert.ToInt16(args[0]));
                Logger.Log("Listening on port ", args[0]);
                Console.Title = "net_47sb_59vm:" + args[0];
            }
            else
            {
                httpServer = new HttpServer(cfg.defaultport);
                Logger.Log("Listening on port " + cfg.defaultport.ToString());
                Console.Title = "net_47sb_59vm:" + cfg.defaultport.ToString();
            }
            LoggerThread = new Thread(new ThreadStart(Logger.LogWorker));
            LoggerThread.Start();
            Listener = new Thread(new ThreadStart(httpServer.listen));
            Listener.Start();
            ConsoleThread = new Thread(new ThreadStart(CmdConsole.Start));
            ConsoleThread.Start();
            ControlThread = new Thread(new ThreadStart(Control));
            return 0;
        }

        public static void PerformFileCheck()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "configs")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "configs"));
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "configs/Main.cfg")))
            {
                Config.Save(cfg);
                cfg.Deserialize();
            }
            else
            {
                cfg.Deserialize();
                Config.Save(cfg);
            }
            Verbose = cfg.verbose;
            HttpProcessor.MAX_POST_SIZE = cfg.maxPOSTmb * 1024 * 1024;

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, cfg.codepath)))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, cfg.codepath));
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "server/404.html")))
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "server/404.html"), "<h1>404 - Not found</h1><br><h5>__________________________________________________________________<br>Hat.NET - an opensource .NET webserver software</h5>".WriteHTMLStub("<title>404</title>"));
        }

        public static void Control()
        {
            while (true)
                if (!ConsoleThread.IsAlive && !LoggerThread.IsAlive)
                {
                    FireExit();
                    Thread.CurrentThread.Abort();
                }
        }

        public static void FireExit() { Exit(null, new HandledEventArgs()); }
    }
}
