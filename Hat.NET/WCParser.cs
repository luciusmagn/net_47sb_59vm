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
                    AttributeType mode = new AttributeType();
                    switch(Path.GetExtension(statement.RemoveIndent().Substring(6)))
                    {
                        case "js":
                            mode = AttributeType.JS;
                            break;
                        case "css":
                            mode = AttributeType.CSS;
                            break;
                        default:
                            mode = AttributeType.UNKNOWN;
                            break;
                    }
                    UsingQuery.Add(string.Format("<{0} {1}={2} {3}={4}>", 
                        (mode == AttributeType.CSS ? "link" : (mode == AttributeType.JS ? "script" : "link")), 
                        (mode == AttributeType.CSS ? "rel" : (mode == AttributeType.JS ? "type" : "rel")), 
                        (mode == AttributeType.CSS ? "stylesheet" : (mode == AttributeType.JS ? "text/javascript" : "text/css")), 
                        (mode == AttributeType.CSS ? "href" : (mode == AttributeType.JS ? "src" : "href")), 
                        statement.RemoveIndent().Substring(6)
                    ));
                    Console.WriteLine(string.Format("<{0} {1}={2} {3}={4}>",
                        (mode == AttributeType.CSS ? "link" : (mode == AttributeType.JS ? "script" : "link")),
                        (mode == AttributeType.CSS ? "rel" : (mode == AttributeType.JS ? "type" : "rel")),
                        (mode == AttributeType.CSS ? "stylesheet" : (mode == AttributeType.JS ? "text/javascript" : "text/css")),
                        (mode == AttributeType.CSS ? "href" : (mode == AttributeType.JS ? "src" : "href")),
                        statement.RemoveIndent().Substring(6)
                    ));
                }
                else if(statement.RemoveIndent().StartsWith("doctype"))
                    BuiltHTML.AppendLine(string.Format("<!doctype {0}>", statement.RemoveIndent().ToLower().Replace("doctype", "").RemoveIndent()));
            }
        }
    }
}
