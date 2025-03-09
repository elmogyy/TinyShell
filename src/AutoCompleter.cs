using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace codecrafters_shell.src
{
    static class AutoCompleter
    {
        private static void BuiltinCompletion(StringBuilder userInput)
        {
            switch (userInput.ToString())
            {
                case "echo":
                    userInput.Append(' ');
                    Console.Write(' ');
                    break;
                case "ech":
                    userInput.Append("o ");
                    Console.Write("o ");
                    break;
                case "exit":
                    userInput.Append(' ');
                    Console.Write(' ');
                    break;
                case "exi":
                    userInput.Append("t ");
                    Console.Write("t ");
                    break;
                case "typ":
                    userInput.Append("e ");
                    Console.Write("e ");
                    break;
                default:
                    Console.Write('\a');
                    break;
            }
        }
        private static void ExecutableCompletion(StringBuilder userInput)
        {
            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            string[] paths = pathVariable != null ? pathVariable.Split(Path.PathSeparator) : Array.Empty<string>();
            List<string> matches = new List<string>();
            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    matches.AddRange(Directory.GetFiles(path)
                        .Select(filePath => Path.GetFileName(filePath))
                        .Where(fileName => fileName.StartsWith(userInput.ToString())).ToList());
                    //files = Directory.GetFiles(path)
                       // .Select(filePath => Path.GetFileName(filePath))
                        //.Where(fileName => fileName.StartsWith(userInput.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();

                    

                    /* string? executable = Directory.GetFiles(path).Select(filePath => Path.GetFileName(filePath))
                 .FirstOrDefault(fileName => fileName.StartsWith(userInput.ToString(), StringComparison.OrdinalIgnoreCase));

                     if (!string.IsNullOrEmpty(executable))
                     {
                         //Console.SetCursorPosition(2, Console.CursorTop);
                         Console.Write("\u001b[3G");
                         Console.Write(executable+" ");
                         userInput.Clear();
                         userInput.Append(executable);

                     }*/
                }
            }
            matches = matches.Distinct().OrderBy(s => s).ToList();
            if (matches.Count == 1)
            {
                CompleteInput(userInput, matches[0]);
                Console.Write(' ');
                userInput.Append(' ');
            }
            else if (matches.Count > 1)
            {
                HandleMultipleMatches(userInput, matches);
            }

        }
        private static void HandleMultipleMatches(StringBuilder userInput, List<string> matches)
        {
            string commonPrefix = GetLongestCommonPrefix(matches);
            CompleteInput(userInput, commonPrefix);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Tab)
            {
                Console.WriteLine();
                PrintMatchingExecutables(matches);
                Console.WriteLine();
                Console.Write($"$ {userInput}");
            }
            else
            {
                Console.Write(keyInfo.KeyChar);
                userInput.Append(keyInfo.KeyChar);
            }
        }
        private static void PrintMatchingExecutables(List<string> matches)
        {
            if (matches.Count == 0) return;
            foreach (string match in matches)
            {
                Console.Write(match + "  ");
            }
        }
        private static void CompleteInput(StringBuilder userInput, string completion)
        {
            for (int i = userInput.Length; i < completion.Length; i++)
            {
                Console.Write(completion[i]);
                userInput.Append(completion[i]);
            }
        }
        private static string GetLongestCommonPrefix(List<string> matches)
        {
            if (matches.Count == 0) return "";

            string prefix = matches[0];
            foreach (string match in matches)
            {
                int i = 0;
                while (i < prefix.Length && i < match.Length && prefix[i] == match[i]) i++;
                prefix = prefix.Substring(0, i);
            }
            return prefix;
        }

        public static void Complete(StringBuilder userInput)
        {
            BuiltinCompletion(userInput);
            ExecutableCompletion(userInput);
        }
    }
}
