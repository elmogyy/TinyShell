using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace codecrafters_shell.src
{
    static class Parser
    {
        static private List<string> ParseArgumentString(string argumentString)
        {
            List<string> arguments = new List<string>();
            StringBuilder currentArgument = new StringBuilder();
            bool inSingleQuote = false, inDoubleQuote = false;
            while (true)
            {
                for (int i = 0; i < argumentString.Length; i++)
                {
                    char currentChar= argumentString[i];
                    if (currentChar == '\\' && !inSingleQuote)
                    { 
                        if (i != argumentString.Length - 1)
                        {
                            char nextChar = argumentString[i + 1];
                            if ((nextChar == '\\' || nextChar == '\'' || nextChar == '\"') && !inDoubleQuote)
                            {
                                currentArgument.Append(nextChar);
                                i++;
                            }
                            else if (nextChar == ' ' && !inDoubleQuote)
                            {
                                currentArgument.Append(' ');
                                i++;
                            }
                            else if ((nextChar == '\\' || nextChar == '\"' || nextChar == '$') && inDoubleQuote)
                            {
                                currentArgument.Append(nextChar);
                                i++;
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (currentChar == '"' && !inSingleQuote)
                    {
                            inDoubleQuote = !inDoubleQuote;
                            continue;  
                    }
                    else if (currentChar == '\'' && !inDoubleQuote)
                    {
                        inSingleQuote = !inSingleQuote;
                        continue; 
                    }
                    else if (currentChar == ' ' && !inSingleQuote && !inDoubleQuote)
                    {
                        if (currentArgument.Length != 0)
                        { 
                            arguments.Add(currentArgument.ToString()); 
                        }
                        currentArgument.Clear();
                        continue;
                    }
                    currentArgument.Append(currentChar);
                }
                if (!inSingleQuote && !inDoubleQuote && argumentString[argumentString.Length-1] != '\\') break;
                Console.Write("> ");
                argumentString = Console.ReadLine()??" ";
            }
            if (currentArgument.Length > 0)
            {
                arguments.Add(currentArgument.ToString());
            }
            return arguments;
        }
        public static void Parse(string? commandLine, out string command, out string[] arguments)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
            {
                command = string.Empty;
                arguments = Array.Empty<string>();
            }
            else
            {
                commandLine = commandLine.Trim();
                arguments = ParseArgumentString(commandLine).ToArray();
                command = arguments[0];
                arguments = arguments.Skip(1).ToArray();
            }       
        }
    }
}
