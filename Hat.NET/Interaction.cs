using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET.Interaction
{
    public delegate void Hook(string fileName, HttpProcessor processor, HandleHaltArgs e);
    public class Interaction
    {
        /// <summary>
        /// String should be the extension of the file you wanna hook. Dot optional.
        /// </summary>
        public static List<ValuePair<string, Hook>> ExtensionHooks = new List<ValuePair<string, Hook>>();

        /// <summary>
        /// String should be the name of the request you wanna hook.
        /// </summary>
        public static List<ValuePair<string, Hook>> NameHooks = new List<ValuePair<string, Hook>>();

        /// <summary>
        /// General hooks. You check for the condition and/or prevent other hooks.
        /// </summary>
        public static List<Hook> GeneralHooks = new List<Hook>();

        /// <summary>
        /// Hooks for POST requests. You check for the condition and/or prevent other hooks.
        /// </summary>
        public static List<Hook> POSTHooks = new List<Hook>();

        public static bool TryExtension(string ext, HttpProcessor p, string name = "")
        {
            name = (name == "" ? Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Substring(1)) : name);
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach(ValuePair<string, Hook> pair in ExtensionHooks)
            {
                if (ext.Replace(".", "") == pair.LeftValue.Replace(".", ""))
                {
                    pair.RightValue(name, p, args);
                    flag = true;
                }
            }
            return flag;
        }

        public static bool TryName(string request, HttpProcessor p)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (ValuePair<string, Hook> pair in NameHooks)
            {
                if ((request.StartsWith("/") ? request.Substring(1) : request) == (pair.LeftValue.StartsWith("/") ? pair.LeftValue.Substring(1) : pair.LeftValue))
                {
                    pair.RightValue(p.http_url, p, args);
                    flag = true;
                }
            }
            return flag;
        }

        public static bool Hooks(HttpProcessor p)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (Hook hook in GeneralHooks)
            {
                hook(p.http_url, p, args);
                if(args.Halt)
                {
                    break;
                }
                if(args.PreventDefault)
                {
                    flag = true;
                }
            }
            return flag;
        }
        static Interaction()
        {
            ExtensionHooks.Add(new ValuePair<string, Hook>(".htm", CommonDelegates.HTML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".html", CommonDelegates.HTML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".png", CommonDelegates.PNG));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".css", CommonDelegates.CSS));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".wc", CommonDelegates.WC));
            NameHooks.Add(new ValuePair<string, Hook>("/console", CommonDelegates.Console));
        }
    }
    public static class CommonDelegates
    {
        public static void HTML(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            System.Console.WriteLine("HTML");
            if (!e.Handled)
            {
                processor.writeSuccess();
                System.Console.WriteLine(filename);
                processor.outputStream.Write(File.ReadAllText(filename));
                processor.outputStream.Flush();
            }
        }

        public static void PNG(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                Logger.Log("picture");
                Logger.Log(filename);
                Stream fs = File.Open(filename, FileMode.Open);
                processor.writeSuccess("image/png");
                fs.CopyTo(processor.outputStream.BaseStream);
                processor.outputStream.BaseStream.Flush();
            }
        }

        public static void CSS(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/css");
                processor.outputStream.Write(File.ReadAllText(filename));
                processor.outputStream.Flush();
            }
        }

        public static void WC(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                WCParser.Parse(File.ReadAllText(filename), processor.outputStream);
            }
        }

        public static void Console(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            processor.writeSuccess("text/plain");
            Logger.ViewLog(processor.outputStream);
            processor.outputStream.Flush();
            Logger.SaveLog();
            return;
        }
    }
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class HandleHaltArgs : HandledEventArgs
    {
        private bool halt = false;
        /// <summary>Gets or sets a value that indicates whether the event handler should break the Hook loop.</summary>
        /// <returns>true if yes; otherwise, false.</returns>
        public bool Halt
        {
            get
            {
                return this.halt;
            }
            set
            {
                this.halt = value;
            }
        }
        private bool preventDefault = false;
        /// <summary>Gets or sets a value that indicates whether the event handler should prevent default behavior</summary>
        /// <returns>true if the default behavior should be prevented; otherwise, false.</returns>
        public bool PreventDefault
        {
            get
            {
                return this.preventDefault;
            }
            set
            {
                this.preventDefault = value;
            }
        }
    }
}
