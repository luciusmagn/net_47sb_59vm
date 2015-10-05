using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    public class WCParser
    {
        public static string halfassed;
        public static List<string> UsingQuery = new List<string>();
        public static StringBuilder BuiltHTML;
        public static void Parse(string source, StreamWriter wr)
        {
            halfassed = Utils.RemoveExtraWS(source);
            List<string> statements = Utils.SplitIfNotCodeBlock(halfassed, ';');
            foreach(string statement in statements)
            {
                if(statement.RemoveIndent().StartsWith("using"))
                {
                    What mode = new What();
                    switch(Path.GetExtension(statement.RemoveIndent().Substring(6)))
                    {
                        case "js":
                            mode = What.JS;
                            break;
                        case "css":
                            mode = What.CSS;
                            break;
                        default:
                            mode = What.UNKNOWN;
                            break;
                    }
                    UsingQuery.Add(string.Format("<{0} {1}={2} {3}={4}>", 
                        (mode == What.CSS ? "link" : (mode == What.JS ? "script" : "link")), 
                        (mode == What.CSS ? "rel" : (mode == What.JS ? "type" : "rel")), 
                        (mode == What.CSS ? "stylesheet" : (mode == What.JS ? "text/javascript" : "text/css")), 
                        (mode == What.CSS ? "href" : (mode == What.JS ? "src" : "href")), 
                        statement.RemoveIndent().Substring(6)
                    ));
                    Console.WriteLine(string.Format("<{0} {1}={2} {3}={4}>",
                        (mode == What.CSS ? "link" : (mode == What.JS ? "script" : "link")),
                        (mode == What.CSS ? "rel" : (mode == What.JS ? "type" : "rel")),
                        (mode == What.CSS ? "stylesheet" : (mode == What.JS ? "text/javascript" : "text/css")),
                        (mode == What.CSS ? "href" : (mode == What.JS ? "src" : "href")),
                        statement.RemoveIndent().Substring(6)
                    ));
                }
                else if(statement.RemoveIndent().StartsWith("doctype"))
                {
                    BuiltHTML.AppendLine(string.Format("<!doctype {0}>", statement.RemoveIndent().ToLower().Replace("doctype", "").RemoveIndent()));
                }
            }
        }
    }
}
