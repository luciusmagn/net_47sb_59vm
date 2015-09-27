using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET.Interaction
{
    public delegate void Hook(HttpProcessor processor, HandleHaltArgs e);
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
        /// General hooks. You check for the condition.
        /// </summary>
        public static List<Hook> GeneralHooks = new List<Hook>();

        public static bool TryExtension(string ext, HttpProcessor p)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach(ValuePair<string, Hook> pair in ExtensionHooks)
            {
                if (ext.Replace(".", "") == pair.LeftValue.Replace(".", ""))
                {
                    pair.RightValue(p, args);
                    flag = true;
                }
            }
            return flag;
        }

        public static bool TryName(string request, HttpProcessor p)
        {
            bool flag = false;
            HandleHaltArgs args = new HandleHaltArgs();
            foreach (ValuePair<string, Hook> pair in ExtensionHooks)
            {
                if ((request.StartsWith("/") ? request.Substring(1) : request) == (pair.LeftValue.StartsWith("/") ? pair.LeftValue.Substring(1) : pair.LeftValue))
                {
                    pair.RightValue(p, args);
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
                hook(p, args);
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
