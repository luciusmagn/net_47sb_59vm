using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hat.NET;
using Hat.NET.Interaction;
using System.IO;

namespace HookTest
{
    [ApiVersion(0, 9)]
    public class HookTest : HatComponent
    {
        public override string Author
        {
            get { return "Lukáš Hozda / magnusi"; }
        }
        public override string Description
        {
            get { return "Tests whether custom hooks are working correctly"; }
        }
        public override string Name
        {
            get { return "Hook Test"; }
        }
        public override Version Version
        {
            get { return new Version(1, 0); }
        }
        public override void Initialize()
        {
            Console.WriteLine("loaded");
            Interaction.NameHooks.Add(new ValuePair<string, Hook>("test", GETHook));
            Interaction.GeneralHooks.Add(GETHook);
            Interaction.POSTHooks.Add(POSTHook);
        }

        public void GETHook(string filename, HttpProcessor p, HandleHaltArgs e)
        {
            Console.WriteLine("[potato]:" + p.socket.Client.RemoteEndPoint + filename);
        }

        public void POSTHook(string filename, HttpProcessor p, StreamReader r, HandleHaltArgs e)
        {
            Console.WriteLine("[postato]: " + p.socket.Client.RemoteEndPoint + filename);
            Console.WriteLine(r.ReadToEnd());
        }
    }
}
