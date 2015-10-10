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
        public static Dictionary<string, string> Subdomains = new Dictionary<string, string>();
        public static SubdomainService cfg = new SubdomainService();
        public static void EntryHook(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if(Subdomains.ContainsValue((processor.httpHeaders["Host"] as string)))
            {

            }
        }

        public static void Initialize()
        {
            bool isComment = false;
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "services", "subdomains.service")))
            {
                foreach(string line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "services", "subdomains.service")))
                {
                    if(line.Replace(" ", "").StartsWith("/*"))
                        isComment = true;
                    if (line.Trim().Contains("*/"))
                        isComment = false;
                    if (line.Split(new char[] { '=' }).Count() >= 2 && !(line.StartsWith("//") && line.StartsWith("*") && line.StartsWith("#")) && !isComment)
                    {
                        Subdomains.Add(line.Split(new char[] { '=' })[0], string.Join("", line.Split(new char[] { '=' }).Skip(1)));
                    }
                }
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
