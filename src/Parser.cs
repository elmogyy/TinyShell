using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace codecrafters_shell.src
{
    static class Parser
    {
        static public List<string> ParseArgumentString(string argumentString)
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
                            if (!inDoubleQuote)
                            {
                                if(nextChar == '\\' || nextChar == '\'' || nextChar == '\"')
                                {
                                    currentArgument.Append(nextChar);
                                    i++;
                                }
                                else if(nextChar == ' ')
                                {
                                    currentArgument.Append(' ');
                                    i++;
                                }  
                                continue;
                            }
                            else if(inDoubleQuote)
                            {
                                //char previousChar = argumentString[i - 1];
                                if (nextChar == '\\' || nextChar == '\"' || nextChar == '$')
                                {
                                    currentArgument.Append(nextChar);
                                    i++;
                                    continue;

                                }
                                /*else if(nextChar == 'n')
                                {
                                    currentArgument.Append(Environment.NewLine);
                                    i++;
                                    continue;

                                }*/
                                /*else if(previousChar != '\\' && previousChar != '\"' && previousChar != '$')
                                {
                                    currentArgument.Append(currentChar);
                                    continue;
                                }*/
                            }
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
                        /*if (currentArgument.Length >= 1 && currentArgument[currentArgument.Length-1] == ' ')
                        {
                            currentArgument.Length--;

                        }*/
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
                argumentString = Console.ReadLine();
            }
            if (currentArgument.Length > 0)
            {
                arguments.Add(currentArgument.ToString());
            }
            return arguments;
        }

        public static void Parse(string? commandLine, out string command, out string[] arguments)
        {
            if (string.IsNullOrEmpty(commandLine))
            {
                command = string.Empty;
                arguments = Array.Empty<string>();
            }
            else
            {
                commandLine = commandLine.Trim();
                int index = commandLine.IndexOf(' ');
                if (index != -1)
                {
                    command = commandLine.Substring(0, index);
                    string argumentString = commandLine.Substring(index + 1);
                    //fullArgument = Regex.Replace(fullArgument, @"\s+(?=(?:[^""']*([""'])(?:.*?\1)?)*[^""']*$)", " ");
                    //arguments = Regex.Split(fullArgument, @" (?=(?:[^']*'[^']*')*[^']*$)");
                    //arguments = fullArgument.Split('\'').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    //arguments = arguments.SelectMany(str => str.Split(' ').Where(s => !string.IsNullOrEmpty(s))).ToArray();
                    //arguments = arguments.SelectMany(str => str.Split(' ').Where(s => !string.IsNullOrEmpty(s))).ToArray();
                    /*string pattern = @"(?:""([^""]*)""|'([^']*)'|[^\s""']+)+";
                    arguments = Regex.Matches(fullArgument, pattern)
                           .Select(m => m.Value.Trim('\"', '\'').Replace("\'\'", "").Replace("\"\"", "")).ToArray();*/
                     arguments = ParseArgumentString(argumentString).ToArray();
                }
                else
                {
                    command = commandLine;
                    arguments = Array.Empty<string>();
                }
                /*string[] inputs = user_input.Split(" ");
                command = inputs[0];
                arguments = inputs.Skip(1).ToArray();*/
            }
        }
    }
}
