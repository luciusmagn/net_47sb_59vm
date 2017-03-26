using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    public static class Utils
    {
        public static string RemoveExtraWS(string source)
        {
            string temp = "";
            foreach(char ch in source)
            {
                char last = 'a';
                switch (ch)
                {
                    case ' ':
                    case '\r':
                    case '\n':
                    case ' ':
                        if(last == ch)
                            continue;
                        else
                        {
                            temp += ch;
                            last = ch;
                            continue;
                        }
                }
            }
            return temp;
        }

        public static List<string> SplitIfNotCodeBlock(string source, char c)
        {
            List<string> temp = new List<string>();
            string tempstr = "";
            bool codeblock = false;
            bool isString = false;
            int indexOne = 0;
            int indexTwo = 0;
            foreach(char ch in source)
                switch(ch)
                {
                    case '{':
                        tempstr += ch;
                        indexOne = isString ? indexOne : indexOne++;
                        codeblock = true;
                        break;
                    case '}':
                        tempstr += ch;
                        indexTwo = isString ? indexTwo : indexTwo++;
                        if (indexOne == indexTwo)
                            codeblock = false;
                        break;
                    case '\"':
                        isString = isString ? false : true;
                        break;
                    default:
                        if(ch == c && !codeblock)
                        {
                            temp.Add(tempstr);
                            tempstr = "";
                        }
                        else
                            tempstr += ch;
                        break;
                }
            if(tempstr != "")
                temp.Add(tempstr);
            return temp;
        }
        public static string RemoveIndent(this string source)
        {
            string temp = "";
            do
            {
                if(source[source.GetEnumerator().Current] == ' ' || source[source.GetEnumerator().Current] == '\t')
                    continue;
                else
                    temp += source[source.GetEnumerator().Current];
            }
            while (source.GetEnumerator().MoveNext());
            return temp;
        }

        public static void WriteBinary(string mime, string filename, HttpProcessor p)
        {
            Stream fs = File.Open(filename, FileMode.Open);
            p.writeSuccess(mime);
            fs.CopyTo(p.outputStream.BaseStream);
            p.outputStream.BaseStream.Flush();
        }
    }
    public static class TextUtils
    {
        public static string WriteHTMLStub(this string source, string head = "")
        {
            return string.Format("<!DOCTYPE HTML>\n<html>\n<head>{0}</head>\n<body>{1}</body>\n</html>", head, source);
        }

        public static void WriteCommon(string mime, string filename, HttpProcessor p)
        {
            p.writeSuccess("text/troff");
            p.outputStream.Write(File.ReadAllText(filename));
            p.outputStream.Flush();
        }
    }
    public enum AttributeType
    {
        JS,
        CSS,
        UNKNOWN
    }
}
