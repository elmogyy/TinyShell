using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_shell.src
{
    static class AutoCompleter
    {
        static private void BuiltinCompletion(StringBuilder userInput)
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
        static public void ExecutableCompletion(StringBuilder userInput)
        {
            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            string[] paths = pathVariable != null ? pathVariable.Split(':') : Array.Empty<string>();
            List<string> files = new List<string>();
            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    files.AddRange(Directory.GetFiles(path)
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
            if (files.Count == 1)
            {
                for (int i = userInput.Length; i < files[0].Length; i++)
                {
                    Console.Write(files[0][i]);
                    userInput.Append(files[0][i]);  
                }
                Console.Write(' ');
                userInput.Append(' ');
                /*Console.Write("\u001b[3G");
                Console.Write(files[0] + " ");
                userInput.Clear();
                userInput.Append(files[0] + " ");*/
            }
            else if(files.Count > 1)
            {
                files.Sort();
                if (ConsoleKey.Tab == Console.ReadKey(true).Key)
                {
                    Console.WriteLine();
                    foreach (string file in files)
                    {
                        Console.Write(file + "  ");
                    }
                    Console.WriteLine();
                    Console.Write($"$ {userInput}");
                }
            }

        }
        static public void Complete(StringBuilder userInput)
        {
            BuiltinCompletion(userInput);
            ExecutableCompletion(userInput);
        }
    }
}
