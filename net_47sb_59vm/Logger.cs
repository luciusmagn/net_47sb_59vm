using System;
using System.IO;
using System.Text;

namespace net_47sb_59vm
{
    /// <summary>
    /// Halcyon logger. Handles logging and saving logs to a file.
    /// </summary>
    public class Logger
    {
        public volatile static StringBuilder LogContent = new StringBuilder();
        public volatile static StringBuilder FullLog = new StringBuilder();
        public static string LogName = "Hat.log";

        /// <summary>
        /// Logs input values into log file, which is saved upon end of the program. 
        /// </summary>
        /// <param name="args"> Anything convertible to string </param>
        public static void Log(params object[] args)
        {
            LogContent.Append("[" + DateTime.Now.ToLongTimeString() + "]");
            FullLog.Append("[" + DateTime.Now.ToLongTimeString() + "]");
            foreach (object arg in args)
            {
                Console.Write(arg);
                LogContent.Append(arg);
                FullLog.Append(arg);
            }
            Console.WriteLine();
            LogContent.AppendLine();
            FullLog.AppendLine();
        }
        /// <summary>
        /// Log showing output to console only when Verbose mode is enabled.
        /// </summary>
        /// <param name="args"></param>
        public static void TalkyLog(params object[] args)
        {
            LogContent.Append("[" + DateTime.Now.ToLongTimeString() + "]");
            FullLog.Append("[" + DateTime.Now.ToLongTimeString() + "]");
            foreach (object arg in args)
            {
                if (Program.Verbose)
                    Console.Write(arg);
                LogContent.Append(arg);
                FullLog.Append(arg);
            }
            if (Program.Verbose)
                Console.WriteLine();
            LogContent.AppendLine();
            FullLog.AppendLine();
        }

        /// <summary>
        /// Logs input values into log file, but does not create a new line.
        /// </summary>
        /// <param name="args"> Anything convertible to string </param>
        public static void LogNoNl(params object[] args)
        {
            foreach (object arg in args)
            {
                Console.Write(arg);
                LogContent.Append(arg);
                FullLog.Append(arg);
            }
        }
        /// <summary>
        /// Logs without telling anyone anything.
        /// </summary>
        /// <param name="args"></param>
        public static void LogNoTrace(params object[] args)
        {
            LogContent.Append("[" + DateTime.Now.ToLongTimeString() + "]");
            FullLog.Append("[" + DateTime.Now.ToLongTimeString() + "]");
            foreach (object arg in args)
                LogContent.Append(arg);
            FullLog.AppendLine();
            LogContent.AppendLine();
        }
        /// <summary>
        /// Inits the logger
        /// </summary>
        static Logger()
        {
            Log("Logger initialized.");
            Program.Exit += (x, y) => { Save(); };
        }

        public static void SaveLog()
        {
            string path = Path.Combine(Environment.CurrentDirectory, LogName);
            string existingContents = "";
            if (File.Exists(path))
                existingContents = File.ReadAllText(path);
            StreamWriter output = new StreamWriter(path);
            output.Write(existingContents);
            output.WriteLine();
            output.Write(LogContent);
            output.Flush();
            output.Close();
            LogContent.Clear();
        }
        /// <summary>
        /// Saves log to a file with custom filename.
        /// </summary>
        /// <param name="filename"></param>
        public static void SaveLog(string filename)
        {
            bool SeparateAdditions = false;
            if (!string.IsNullOrEmpty(filename) && !string.IsNullOrWhiteSpace(filename))
            {
                string path = Path.Combine(Environment.CurrentDirectory, filename);
                string existingContents = "";
                if (File.Exists(path))
                {
                    existingContents = File.ReadAllText(path);
                    SeparateAdditions = true;
                }
                StreamWriter output = new StreamWriter(path);
                output.Write(existingContents);
                if (SeparateAdditions)
                {
                    output.WriteLine("\n");
                    output.WriteLine("\n");
                }
                output.Write("\n" + DateTime.Now.ToLongTimeString() + "\n");
                output.Write(LogContent);
                output.Close();
                LogContent.Clear();
            }
        }
        public static void ViewLog(StreamWriter p) { p.WriteLine(FullLog.ToString()); }

        public static void LogWorker()
        {
            while (true)
                if(Program.PendingLogSave)
                {
                    SaveLog();
                    Program.PendingLogSave = false;
                }
        }

        public static void Save()
        {
            if(Program.cfg.asynchlog)
                Program.PendingLogSave = true;
            else
                SaveLog();
        }
    }
}
