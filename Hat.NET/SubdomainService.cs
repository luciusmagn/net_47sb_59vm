using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hat.NET.Interaction;
using System.Collections;
using System.IO;
using Hat.NET.Configs;

namespace Hat.NET
{
    public class SubdomainService
    {
        Dictionary<string, string> Subdomains = new Dictionary<string, string>();
        public static SubdomainService cfg = new SubdomainService();
        public static void EntryHook(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            
        }

        public static void Initialize()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "services", "subdomains.service")))
            {
                //foreach(string line in )
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
