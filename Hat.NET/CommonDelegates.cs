using net_47sb_59vm.Interaction;
using System.IO;

namespace net_47sb_59vm
{
    public static class Delegates
    {
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

        public static void Console(string fileName, HttpProcessor processor, HandleHaltArgs e)
        {
            processor.writeSuccess("text/plain");
            Logger.ViewLog(processor.outputStream);
            processor.outputStream.Flush();
            Logger.Save();
            return;
        }

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
