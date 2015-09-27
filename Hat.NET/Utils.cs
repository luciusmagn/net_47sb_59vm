using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    public static class Utils
    {
        public static List<string> GetCodeBlocks(string source)
        {
            List<string> Blocks = new List<string>();
            string CurrentBlock = "";
            bool isInString = false;
            string Temp;
            int OpenedBrackets = 0;
            int ClosedBrackets = 0;
            int charIndex = 0;
            Temp = source;
            NEXTBLOCK:
            foreach (char ch in Temp)
            {
                if (And(ch == '\"' || ch == '\'', (charIndex != 0 ? source[charIndex - 1] != '\\' : true)))
                {
                    if (isInString) isInString = false;
                    else isInString = true;
                }
                if (ch == '{' && !isInString)
                {
                    CurrentBlock += ch;
                    OpenedBrackets++;
                }
                else if (ch == '}' && !isInString)
                {
                    CurrentBlock += ch;
                    ClosedBrackets++;
                    if (And(OpenedBrackets != 0, ClosedBrackets != 0) && OpenedBrackets == ClosedBrackets)
                    {
                        break;
                    }
                }
                else
                {
                    CurrentBlock += ch;
                }
                charIndex++;
            }
            Temp = Temp.Replace(CurrentBlock, "");
            Blocks.Add(CurrentBlock);
            CurrentBlock = "";
            if (!String.IsNullOrWhiteSpace(Temp))
            {
                goto NEXTBLOCK;
            }
            return Blocks;
        }
        public static bool And(bool left, bool right)
        {
            if (right && left)
            {
                return true;
            }
            else { return false; }
        }

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
                        {
                            continue;
                        }
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
            {
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
                        {
                            codeblock = false;
                        }
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
                        {
                            tempstr += ch;
                        }
                        break;

                }
            }
            return temp;
        }
        public static string RemoveIndent(this string source)
        {
            string temp = "";
            do
            {
                if(source[source.GetEnumerator().Current] == ' ' || source[source.GetEnumerator().Current] == '\t')
                {
                    continue;
                }
                else
                {
                    temp += source[source.GetEnumerator().Current];
                }
            }
            while (source.GetEnumerator().MoveNext());
            return temp;
        }
        public static string WriteHTMLStub(string body, string head = "")
        {
            return string.Format("<!DOCTYPE HTML>\n<html>\n<head>{0}</head>\n<body>{1}</body>\n</html>", head, body);
        }
    }
    public static class StringUtils
    {
        public static string WriteHTMLStub(this string source, string head = "")
        {
            return string.Format("<!DOCTYPE HTML>\n<html>\n<head>{0}</head>\n<body>{1}</body>\n</html>", head, source);
        }
    }
    public enum What
    {
        JS,
        CSS,
        UNKNOWN
    }
}
