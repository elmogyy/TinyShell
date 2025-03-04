using System;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace codecrafters_shell.src
{
    static class Parser
    {
        static private void HandleFileRedirection(string fileName, bool append)
        {

            if (!File.Exists(fileName) || append == false)
            {
                File.Create(fileName).Dispose();
                //File.WriteAllText(fileName, "");
            }
        }
       /* static private void HandleRedirection(List<string> argumentsList)
        {
            foreach (string argument in argumentsList)
            {
                if (argument == ">" || argument == "1>") HandleFileRedirection(argument+1, false);
                else if(argument == ">>" || argument == "1>>") HandleFileRedirection(argument + 1, true);
            }
            HashSet<string> redirections = new HashSet<string> { ">", "1>", ">>", "1>>" };
            argumentsList.RemoveAll(argument => redirections.Contains(argument));
           // argumentsList.RemoveAll(argument => (argument == ">" || argument == "1>" || argument == ">>" || argument == "1>>"));

        }*/
        /*static private void ParseRedirectInNonQouteString(List<string> argumentsList)
        {
        }
            static private void ParseRedirectString(List<string> argumentsList)
        {
            foreach (string argument in argumentsList)
            {
                if (argument.StartsWith('\"') || argument.StartsWith('\'')) continue;
                argument = "dasf";
                argumentsList.ins
            }
        }*/
        public enum RedirectType 
        {
            stdout = '1',
            stderr = '2'
        }
        static private bool TryParseNextRedirect(ref string commandLine, out string fileName , out bool append, out RedirectType redirectType)
        {
            StringBuilder commandLineWithoutRedirect = new StringBuilder();
            bool inSingleQuote = false, inDoubleQuote = false;
            for (int i = 0; i < commandLine.Length; i++)
            {
                char currentChar = commandLine[i];
                if (currentChar == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    continue;
                }
                else if (currentChar == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    continue;
                }
                else if (currentChar == '>' && !inSingleQuote && !inDoubleQuote)
                {
                    if (i == commandLine.Length - 1  || (i == commandLine.Length - 2 && commandLine[i + 1] == '>')) throw new Exception("syntax error near unexpected token `newline'");
                    if (commandLine[i + 1] == '>')
                    {
                        commandLine = commandLine.Substring(i + 2);
                        append = true;
                    }
                    else
                    {
                        commandLine = commandLine.Substring(i + 1);
                        append = false;
                    }
                    fileName = ParseNextArgument(ref commandLine);
                    if (fileName.StartsWith(">>")) throw new Exception("syntax error near unexpected token `>>'");
                    if (fileName.StartsWith('>')) throw new Exception("syntax error near unexpected token `>'");
                    if(commandLineWithoutRedirect.Length == 1 && (commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 1] == '1' || commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 1] == '2'))
                    {
                        redirectType = (RedirectType)commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 1];
                        commandLineWithoutRedirect.Remove(commandLineWithoutRedirect.Length - 1, 1);

                        //commandLineWithoutRedirect.Length--;

                    }
                    else if(commandLineWithoutRedirect.Length > 1 && commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 2] == ' ' && (commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 1] == '1' || commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 1] == '2'))
                    {
                        redirectType = (RedirectType)commandLineWithoutRedirect[commandLineWithoutRedirect.Length - 1];
                        commandLineWithoutRedirect.Remove(commandLineWithoutRedirect.Length - 1, 1);

                       // commandLineWithoutRedirect.Length--;
                    }
                    else
                    {
                        redirectType = RedirectType.stdout;
                    }
                    //Console.WriteLine(fileName);
                    //Console.WriteLine(append);
                    //Console.WriteLine(redirectType);
                    commandLine = commandLineWithoutRedirect.ToString()+ " " + commandLine;
                   // Console.WriteLine(commandLine);
                    return true;
                }
                commandLineWithoutRedirect.Append(currentChar);
            }
            fileName = "";
            append = false;
            redirectType = 0;
            return false;
        }

        static private string ParseNextArgument(ref string commandLine)
        {
            StringBuilder currentArgument = new StringBuilder();
            bool inSingleQuote = false, inDoubleQuote = false;
            while (true)
            {
                for (int i = 0; i < commandLine.Length; i++)
                {
                    char currentChar= commandLine[i];
                    if (currentChar == '\\' && !inSingleQuote)
                    { 
                        if (i != commandLine.Length - 1)
                        {
                            char nextChar = commandLine[i + 1];
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

                   /* else if (currentChar == ' ' && nextChar == '>' && !inDoubleQuote)
                    {
                        currentArgument.Append(' ');
                        currentArgument.Append('>');
                        i++;
                    }*/
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
                    /*else if (currentChar == '>' && !inSingleQuote && !inDoubleQuote)
                    {
                        inRedirect = true;
                        string redirectString = argumentString.Substring(i);
                        ParseRedirectString(redirectString);



                    }*/


                    else if ((currentChar == ' ' ) && !inSingleQuote && !inDoubleQuote)
                    {
                        if (currentArgument.Length != 0)
                        {
                            commandLine = commandLine.Substring(i+1);
                            return currentArgument.ToString();
                        }
                        continue;
                        
                    }
                    currentArgument.Append(currentChar);
                }
                if (!inSingleQuote && !inDoubleQuote && commandLine[commandLine.Length-1] != '\\') break;
                Console.Write("> ");
                commandLine = Console.ReadLine()??" ";
            }
            commandLine = "";
            return currentArgument.ToString(); 
        }
        public static void Parse(string? commandLine, out string command, out string[] arguments, ref string outputDestination, ref string errorDestination)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
            {
                command = string.Empty;
                arguments = Array.Empty<string>();
            }
            else
            {
                commandLine = commandLine.Trim();
                bool isNextRedirectValid = true;
                while (isNextRedirectValid)
                {
                    string fileName;
                    RedirectType redirectType;
                    bool append;
                    isNextRedirectValid = TryParseNextRedirect(ref commandLine, out fileName , out append, out redirectType);
                    if (isNextRedirectValid && redirectType == RedirectType.stdout)
                    {
                        outputDestination = fileName;
                        HandleFileRedirection(fileName, append);
                    }
                    else if (isNextRedirectValid && redirectType == RedirectType.stderr)
                    {
                        errorDestination = fileName;
                        HandleFileRedirection(fileName, append);
                    }
                }
               /* do
                {
                   RedirectType? redirectType = null;
                   bool? append = null;
                   bool isNextRedirectValid = TryParseNextRedirect(ref commandLine,ref append, ref redirectType);
                   if(nextRedirect != "")
                   {
                        HandleFileRedirection(nextRedirect, append);
                   }
                    
                } while (!string.IsNullOrEmpty(redirect));*/
                List<string> argumentsList = new List<string>();
                while (!String.IsNullOrEmpty(commandLine))
                {
                    argumentsList.Add(ParseNextArgument(ref commandLine));
                }
                command = argumentsList[0];
                arguments = argumentsList.Skip(1).ToArray();

                /*List<string> argumentsList = ParseArgumentString(commandLine);
                HandleRedirection(argumentsList);
                arguments = argumentsList.ToArray();
               
                arguments = arguments.Skip(1).ToArray();*/

            }       
        }
    }
}
