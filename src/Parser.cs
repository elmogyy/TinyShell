using System.Linq;
using System.Text.RegularExpressions;

namespace codecrafters_shell.src
{
    static class Parser
    {
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
                    string fullArgument = commandLine.Substring(index + 1);
                    fullArgument = fullArgument.Trim();
                    //fullArgument = Regex.Replace(fullArgument, @"\s+(?=(?:[^""']*([""'])(?:.*?\1)?)*[^""']*$)", " ");
                    //arguments = Regex.Split(fullArgument, @" (?=(?:[^']*'[^']*')*[^']*$)");
                    //arguments = fullArgument.Split('\'').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    //arguments = arguments.SelectMany(str => str.Split(' ').Where(s => !string.IsNullOrEmpty(s))).ToArray();
                    //arguments = arguments.SelectMany(str => str.Split(' ').Where(s => !string.IsNullOrEmpty(s))).ToArray();
                    string pattern = @"(?:""([^""]*)""|'([^']*)'|[^\s""']+)+";
                    arguments = Regex.Matches(fullArgument, pattern)
                           .Select(m => m.Value.Trim('\"', '\'').Replace("\'\'", "").Replace("\"\"", "")).ToArray();
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
