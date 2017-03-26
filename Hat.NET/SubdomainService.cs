using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hat.NET.Interaction;
using System.Collections;
using System.IO;

namespace Hat.NET
{
    public class SubdomainService
    {
        public static Dictionary<string, string> Subdomains = new Dictionary<string, string>();
        public static SubdomainService cfg = new SubdomainService();
        public static void EntryHook(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            string str = (string)processor.httpHeaders["Host"];
            Console.WriteLine((str.Contains(":") ? str.Substring(0, str.IndexOf(":")) : str));
            if (Subdomains.ContainsKey((str.Contains(":") ? str.Substring(0, str.IndexOf(":")) : str)))
            {
                e.PreventDefault = true;
                HandleRequest(processor, Subdomains[(str.Contains(":") ? str.Substring(0, str.IndexOf(":")) : str)]);
            }
        }

        public static void HandleRequest(HttpProcessor p, string path)
        {
            if (Interaction.Interaction.TryName(p.http_url, p))
            {
                return;
            }
            //If file doesn't exist try trying it as a folder
            if (Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))) && !File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))))
            {
                p.http_url += (p.http_url == "/" ? "" : "/");
                Logger.Log(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url));
                string[] files = Directory.GetFiles(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), (p.http_url.Length == 1 ? "" : p.http_url.Substring(1))));
                bool flag = false;
                foreach (string filename in files)
                    if (Path.GetFileName(filename).Contains("index"))
                    {
                        Console.WriteLine(filename);
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
                if (!flag)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<a href=\"../\">../</a><br>");
                    foreach (string file in files)
                        sb.AppendLine(string.Format("<a href=\"{0}\">{0}</a><br>", file.Replace(Path.Combine(Environment.CurrentDirectory, path), "")));
                    p.outputStream.WriteLine(sb.ToString());
                    p.writeSuccess();
                    p.outputStream.Flush();
                    return;
                }
                return;
            }
            //if everything else fails, try 404
            else if (!Directory.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))) && !File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url.Length == 1 ? "" : p.http_url.Substring(1))))
            {
                HttpServer.handle404(p);
                return;
            }
            if (!Interaction.Interaction.TryExtension(Path.GetExtension(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url.Substring(1))), p, Path.Combine(Environment.CurrentDirectory, path, p.http_url.Substring(1))))
            {
                p.writeSuccess("text/plain");
                p.outputStream.Write(File.ReadAllText(Path.Combine(Path.Combine(Environment.CurrentDirectory, path), p.http_url.Substring(1))));
                p.outputStream.Flush();
            }
            Logger.Save();
        }
        public static void Initialize()
        {
            bool isComment = false;
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "services", "service.subdomains")))
            {
                foreach (string line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "services", "service.subdomains")))
                {
                    if (line.Replace(" ", "").StartsWith("/*"))
                        isComment = true;
                    if (line.Trim().Contains("*/"))
                        isComment = false;
                    if (line.Split(new char[] { '=' }).Count() >= 2 && !(line.StartsWith("//") && line.StartsWith("*") && line.StartsWith("#")) && !isComment)
                        Subdomains.Add(line.Split(new char[] { '=' })[0], string.Join("", line.Split(new char[] { '=' }).Skip(1)));
                }
            }
            else
            {
                Logger.Log("subdomains file not found. Generating again");
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "services")))
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "services"));
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "services", "service.subdomains"),
                    @"#This is the file for configurations of subdomains
#It should follow this syntax:
#   1. One entry per line;
#   2. No spaces between name, = and path
#   3. The path is relative and shouldn't start with a \\
#example entry:
test.localhost=banana\test
                    ");
            }
        }
    }


}

namespace Hat.NET.Configs
{
    public class SubdomainService : Config
    {
        public bool enabled = true;
    }
}
