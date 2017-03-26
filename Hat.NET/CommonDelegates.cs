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
            if (!e.Handled)
            {
                processor.writeSuccess();
                Logger.Log(filename);
                processor.outputStream.Write(File.ReadAllText(filename));
                processor.outputStream.Flush();
            }
        }
        
        //WC
        public static void WC(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
                WCParser.Parse(File.ReadAllText(filename), processor.outputStream);
        }

    }

    public static class RequestDelegates
    {
        public static void Console(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            processor.writeSuccess("text/plain");
            Logger.ViewLog(processor.outputStream);
            processor.outputStream.Flush();
            Logger.Save();
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

        public static void JPEG(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                Stream fs = File.Open(fileName, FileMode.Open);
                processor.writeSuccess("image/jpeg");
                fs.CopyTo(processor.outputStream.BaseStream);
                processor.outputStream.BaseStream.Flush();
            }
        }

        public static void GIF(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                Stream fs = File.Open(fileName, FileMode.Open);
                processor.writeSuccess("image/gif");
                fs.CopyTo(processor.outputStream.BaseStream);
                processor.outputStream.BaseStream.Flush();
            }
        }
    }
    public static class AppDelegates
    {
        public static void OCTET_STREAM(string filename, HttpProcessor processor, HandleHaltArgs e)
        {
            if (!e.Handled)
            {
                Stream fs = File.Open(filename, FileMode.Open);
                processor.writeSuccess("aplication/octet-stream");
                fs.CopyTo(processor.outputStream.BaseStream);
                processor.outputStream.BaseStream.Flush();
            }
        }
    }
}
