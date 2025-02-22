using System.Linq;
using System.Text.RegularExpressions;

namespace codecrafters_shell.src
{
    static class Parser
    {
        public static void Parse(string? user_input, out string command, out string[] arguments)
        {
            if (string.IsNullOrEmpty(user_input))
            {
                command = string.Empty;
                arguments = Array.Empty<string>();
            }
            else
            {
                user_input = user_input.Trim();
                string[] inputs = user_input.Split(" ");
                command = inputs[0];
                arguments = inputs.Skip(1).ToArray();
                
            }
        }
    }
}
