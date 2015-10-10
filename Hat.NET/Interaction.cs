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
                if (ext.Replace(".", "").ToLower() == pair.LeftValue.Replace(".", "").ToLower())
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

        public static bool POST(HttpProcessor p)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (Hook hook in POSTHooks)
            {
                hook(p.http_url, p, args);
                if (args.Halt)
                {
                    break;
                }
                if (args.PreventDefault)
                {
                    flag = true;
                }
            }
            return flag;
        }
        static Interaction()
        {
            ExtensionHooks.Add(new ValuePair<string, Hook>(".png", ImageDelegates.PNG));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".jpg", ImageDelegates.JPEG));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".jpeg", ImageDelegates.JPEG));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".gif", ImageDelegates.GIF));
            
            ExtensionHooks.Add(new ValuePair<string, Hook>(".htm", TextDelegates.HTML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".html", TextDelegates.HTML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".css", TextDelegates.CSS));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".wc", TextDelegates.WC));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".s", TextDelegates.S));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".par", TextDelegates.PAR));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".csv", TextDelegates.CSV));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".n3", TextDelegates.N3));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".ics", TextDelegates.ICS));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".txt", TextDelegates.TXT));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".dsc", TextDelegates.DSC));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".rtx", TextDelegates.RTX));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".sgml", TextDelegates.SGML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".tsv", TextDelegates.TSV));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".t", TextDelegates.T));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".ttl", TextDelegates.TTL));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".uri", TextDelegates.URI)); 
            ExtensionHooks.Add(new ValuePair<string, Hook>(".curl", TextDelegates.CURL));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".dcurl", TextDelegates.DCURL));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".mcurl", TextDelegates.MCURL));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".scurl", TextDelegates.SCURL));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".fly", TextDelegates.FLY));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".flx", TextDelegates.FLX));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".gv", TextDelegates.GV));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".3dml", TextDelegates._3DML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".spot", TextDelegates.SPOT));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".jad", TextDelegates.JAD));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".wml", TextDelegates.WML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".wmls", TextDelegates.WMLS));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".c", TextDelegates.C));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".f", TextDelegates.F));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".java", TextDelegates.JAVA));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".p", TextDelegates.P));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".etx", TextDelegates.ETX));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".vcs", TextDelegates.VCS));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".uu", TextDelegates.UU));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".vcf", TextDelegates.VCF));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".yaml", TextDelegates.YAML));

            ExtensionHooks.Add(new ValuePair<string, Hook>(".exe", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".bin", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".arc", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".arj", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".com", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".dump", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".lha", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".lhx", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".lzh", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".lzx", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".psd", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".saveme", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".uu", AppDelegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".zoo", AppDelegates.OCTET_STREAM));

            NameHooks.Add(new ValuePair<string, Hook>("/console", RequestDelegates.Console));

            GeneralHooks.Add(SubdomainService.EntryHook);
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
