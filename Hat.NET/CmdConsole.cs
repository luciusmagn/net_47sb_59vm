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
            while(true)
            {
                Console.Write(Program.consolecfg.ConsoleReadyString);
                CommandSelector(Console.ReadLine());
            }
        }

        public static void CommandSelector(string line)
        {
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
                            command.Substring(cmd.LeftValue.Length - 1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            );
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (!command.StartsWith(" "))
                        Logger.TalkyLog("No commands with the specified name found: " + (command.Contains(" ") ? command.Substring(command.IndexOf(' ') - 1) : command));
                    else
                        Logger.TalkyLog("Command mustn't start with a space");
                }
            }
        }
    }
    public static class CommonCommands
    {
        public static void Help(string[] args)
        {
        }
    }
}
