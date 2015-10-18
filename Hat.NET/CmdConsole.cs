using System;
using System.Collections.Generic;
namespace Hat.NET
{
    public delegate void Command(string[] args);
    public static class CmdConsole
    {
        public static List<ValuePair<string, Command>> CmdList = new List<ValuePair<string, Command>>();
        public static List<string> HelpStrings = new List<string>();
        public static char CommandDelimiter = Program.consolecfg.CommandDelimiter;

        public static void Start()
        {
            CommonCommands.isInitialized = true;
            while(true)
            {
                Console.Write(Program.consolecfg.ConsoleReadyString);
                CommandSelector(Console.ReadLine());
            }
        }

        public static void CommandSelector(string line)
        {
            Logger.LogNoTrace(Program.consolecfg.ConsoleReadyString + line);
            string temp = "";
            int index = 0;
            bool isString = false;
            bool found = false;
            char precedingchar = '\n';
            List<string> commands = new List<string>();
            foreach (char ch in line)
            {
                if (ch == '\"')
                {
                    if (isString) isString = false;
                    else isString = true;
                    temp += ch;
                }
                if (ch == CommandDelimiter && index != 0 && line[index] != '\\' && !isString && precedingchar != CommandDelimiter)
                {
                    commands.Add(temp);
                    temp = "";
                }
                else
                {
                    if (precedingchar + ch != CommandDelimiter + ' ')
                    {
                        temp += ch;
                    }
                }
                    
                if(index == line.Length - 1 && ch != CommandDelimiter)
                {
                    commands.Add(temp);
                    temp = "";
                }
                index++;
                precedingchar = ch;
            }
            foreach (string command in commands)
            {
                foreach (ValuePair<string, Command> cmd in CmdList)
                {
                    if (command.StartsWith(cmd.LeftValue))
                    {
                        cmd.RightValue.Invoke(
                            command.Substring(cmd.LeftValue.Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            );
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (!command.StartsWith(" "))
                        Logger.TalkyLog("No commands with the specified name found: " + (command.Contains(" ") ? command.Substring(0, command.IndexOf(' ')) : command));
                    else
                        Logger.TalkyLog("Command mustn't start with a space");
                }
            }
            Logger.Save();
        }
    }
    public static class CommonCommands
    {
        public static bool isInitialized = false;
        static CommonCommands()
        {
            CmdConsole.CmdList.Add(new ValuePair<string, Command>("help", Help));
            CmdConsole.CmdList.Add(new ValuePair<string, Command>("test", Test));
        }
        public static void Help(string[] args)
        {
            Logger.Log("Available commands:");
            Logger.Log("===================");
            foreach(ValuePair<string, Command> cmd in CmdConsole.CmdList)
            {
                Console.Write(cmd.LeftValue + ", ");
            }
            Logger.Log();
        }

        public static void Test(string[] args)
        {
            foreach(string arg in args)
            {
                Logger.Log(arg);
            }
        }
    }
}
