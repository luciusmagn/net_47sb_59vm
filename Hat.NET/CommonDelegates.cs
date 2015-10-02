using Hat.NET.Interaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    /// <summary>
    /// Every content type that starts with text/
    /// </summary>
    public static class TextDelegates
    {
        //HTML
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
        //CSS
        public static void CSS(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/css");
                processor.outputStream.Write(File.ReadAllText(filename));
                processor.outputStream.Flush();
            }
        }
        
        //WC
        public static void WC(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                WCParser.Parse(File.ReadAllText(filename), processor.outputStream);
            }
        }

        

        //Assembly source file
        public static void S(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-asm");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //BAS Partitur Format
        public static void PAR(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/plain-bas");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //CSV - Comma Separated Values
        public static void CSV(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/csv");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Notation3
        public static void N3(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/n3");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //ICalendar
        public static void ICS(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/calendar");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Text File
        public static void TXT(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/plain");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //PRS Lines Tag
        public static void DSC(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/prs.lines.tag");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Rich text format
        public static void RTX(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/rich-text");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //TROFF
        public static void T(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/troff");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Standard Generalized Markup Language
        public static void SGML(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/sgml");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Curl - Applet
        public static void CURL(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.curl");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //mod_fly
        public static void FLY(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.fly");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }
        
        //In3D
        public static void SPOT(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {

            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.in3d.spot");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Fortran source file
        public static void F(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-fortran");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //vCalendar
        public static void VCS(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-vcalendar");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Yet another markup language
        public static void YAML(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/yaml");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //vCard
        public static void VCF(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-vcard");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //UUEncode
        public static void UU(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-uuencode");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Setext
        public static void ETX(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-setex");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Pascal source file
        public static void P(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-pascal");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Java source file
        public static void JAVA(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-java-source.java");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //C Source file
        public static void C(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/x-c");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Wireless Markup Language Script
        internal static void WMLS(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.wap.wmlscript");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Wireless Markup Language
        public static void WML(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.wap.wml");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //J2ME App Descriptor
        public static void JAD(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.sun.j2me.appdescriptor");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //In3D
        public static void _3DML(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.in3d.3dml");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //FLEXSTOR
        public static void FLX(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.fml.flexstor");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Graphiz
        public static void GV(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.grapiz");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Curl - Source Code
        public static void SCURL(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.curl");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Curl - Manifest File
        public static void MCURL(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.curl.dcurl");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Curl - Detachet Applet
        public static void DCURL(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/vnd.curl.dcurl");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //URI RESOLUTION SERVICES
        public static void URI(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/uri-list");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Turtle
        public static void TTL(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/turtle");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

        //Tab separated values
        public static void TSV(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                processor.writeSuccess("text/tab-separated-values");
                processor.outputStream.Write(File.ReadAllText(fileName));
                processor.outputStream.Flush();
            }
        }

    }

    public static class RequestDelegates
    {
        public static void Console(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            processor.writeSuccess("text/plain");
            Logger.ViewLog(processor.outputStream);
            processor.outputStream.Flush();
            Logger.SaveLog();
            return;
        }
    }

    //For those content-types that start with image/
    public static class ImageDelegates
    {
        //PNG
        public static void PNG(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                Stream fs = File.Open(filename, FileMode.Open);
                processor.writeSuccess("image/png");
                fs.CopyTo(processor.outputStream.BaseStream);
                processor.outputStream.BaseStream.Flush();
            }
        }
    }
}
