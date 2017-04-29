using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;

namespace net_47sb_59vm.Interaction
{
    public delegate void Hook(string fileName, HttpProcessor processor, HandleHaltArgs e);
    public delegate void POSTHook(string fileName, HttpProcessor processor, StreamReader inputData, HandleHaltArgs e);
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
        public static List<POSTHook> POSTHooks = new List<POSTHook>();

        public static bool TryExtension(string ext, HttpProcessor p, string name = "")
        {
            name = (name == "" ? Path.Combine(Path.Combine(Environment.CurrentDirectory, "server"), p.http_url.Substring(1)) : name);
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (ValuePair<string, Hook> pair in ExtensionHooks)
                if (ext.Replace(".", "").ToLower() == pair.LeftValue.Replace(".", "").ToLower())
                {
                    pair.RightValue(name, p, args);
                    flag = true;
                }
            return flag;
        }

        public static bool TryName(string request, HttpProcessor p)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (ValuePair<string, Hook> pair in NameHooks)
                if ((request.StartsWith("/") ? request.Substring(1) : request) == (pair.LeftValue.StartsWith("/") ? pair.LeftValue.Substring(1) : pair.LeftValue))
                {
                    pair.RightValue(p.http_url, p, args);
                    flag = true;
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
                if (args.Halt)
                    break;
                if (args.PreventDefault)
                    flag = true;
            }
            return flag;
        }

        public static bool POST(HttpProcessor p, StreamReader data)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (POSTHook hook in POSTHooks)
            {
                hook(p.http_url, p, data, args);
                if (args.Halt)
                    break;
                if (args.PreventDefault)
                    flag = true;
            }
            return flag;
        }
        static Interaction()
        {
            ExtensionHooks.Add(new ValuePair<string, Hook>(".png", (x, y, z) => { if (!z.Handled) Utils.WriteBinary("image/png", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".jpg", (x, y, z) => { if (!z.Handled) Utils.WriteBinary("image/jpeg", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".jpeg", (x, y, z) => { if (!z.Handled) Utils.WriteBinary("image/jpeg", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".gif", (x, y, z) => { if (!z.Handled) Utils.WriteBinary("image/gif", x, y); }));

            ExtensionHooks.Add(new ValuePair<string, Hook>(".htm", Delegates.HTML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".html", Delegates.HTML));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".css", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/css", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".s", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-asm", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".csv", (x, y, z) => TextUtils.WriteCommon("text/csv", x, y)));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".ics", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/calendar", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".txt", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/plain", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".rtx", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/rich-text", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".sgml", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/sgml", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".tsv", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/tab-separated-values", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".t", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/troff", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".ttl", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/turtle", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".uri", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/uri-list", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".c", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-c", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".f", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-fortran", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".p", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-pascal", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".etx", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-setex", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".vcs", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-vcalendar", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".uu", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-uuencode", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".vcf", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/x-vcard", x, y); }));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".yaml", (x, y, z) => { if (!z.Handled) TextUtils.WriteCommon("text/yaml", x, y); }));

            ExtensionHooks.Add(new ValuePair<string, Hook>(".exe", Delegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".bin", Delegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".com", Delegates.OCTET_STREAM));
            ExtensionHooks.Add(new ValuePair<string, Hook>(".dump", Delegates.OCTET_STREAM));

            NameHooks.Add(new ValuePair<string, Hook>("/console", Delegates.Console));

            GeneralHooks.Add(SubdomainService.EntryHook);
        }
    }

    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class HandleHaltArgs : HandledEventArgs
    {
        public bool Halt = false;
        public bool PreventDefault = false;
    }
}
