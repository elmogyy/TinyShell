using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace codecrafters_shell.src
{ 
    public enum CommandType
    {
        exit,
        echo,
        type,
        pwd,
        cd,
    }
    /*readonly struct CommandData
    {
        public string Name { get; }
        public string Description { get; }
        public int NumOfArguments { get; }
        public CommandData(string name,string description,int num_of_arguments)
        {
            Name = name;
            Description = description;
            NumOfArguments = num_of_arguments;
        }
    }
    static class CommandRegistry
    {
        readonly static public CommandData[] Commands = new CommandData[2];
        static CommandRegistry()
        {
            Commands[(int)CommandType.exit] = new CommandData(CommandType.exit.ToString(), "description not exist" ,1);
            Commands[(int)CommandType.echo] = new CommandData(CommandType.echo.ToString(), "description not exist" , 1);
        }
    }*/
    static class CommandHandler
    {
        public static void RunShellOrExecutable(string command, string[] arguments)
        {
            
            if (IsBuiltInShellCommand(command)) { RunShellBuiltInCommand(command, arguments); }
            else
            {
                string executablePath = GetExecutablePath(command);
                if (!string.IsNullOrEmpty(executablePath))
                {
                    RunExecutableFile(command, arguments);
                }
                else
                {
                    throw new Exception($"{command}: command not found");
                    //Console.WriteLine($"{command}: command not found");
                }
            }
        }
        private static void RunShellBuiltInCommand(string command, string[] arguments)
        {
            switch (command)
            {
                case "exit":
                    exit(arguments);
                    break;
                case "echo":
                    echo(arguments);
                    break;
                case "type":
                    type(arguments);
                    break;
                case "pwd":
                    pwd();
                    break;
                case "cd":
                    cd(arguments);
                    break;
                default:
                    throw new Exception($"{command}: command not found");
                    //Console.WriteLine($"{command}: command not found");
                    //break;   
            }
        }
        private static void RunExecutableFile(string command, string[] arguments)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = string.Join(" ", arguments.Select(argument => $"\"{argument.Replace("\"", "\\\"")}\""));

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Console.WriteLine(e.Data); };
                process.ErrorDataReceived += (sender, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Console.Error.WriteLine(e.Data); };

                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private static void exit(string[] arguments)
        {
            if (arguments[0] == "0")
            {
                Environment.Exit(0);
            }
        }
        private static void echo(string[] arguments)
        {
           for (int i = 0; i < arguments.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(arguments[i])) 
                 {
                     Console.Write($"{arguments[i]}"); 
                 }
                 else
                 {
                     Console.Write($"{arguments[i]} ");
                 }
            }
            Console.WriteLine();
        }
        private static void type(string[] arguments)
        {
            if (IsBuiltInShellCommand(arguments[0]))
            {
                Console.WriteLine($"{arguments[0]} is a shell builtin");
            }
            else 
            {
                string executablePath = GetExecutablePath(arguments[0]);
                if (!string.IsNullOrEmpty(executablePath))
                {
                    Console.WriteLine($"{arguments[0]} is {executablePath}");
                }
                else
                {
                    throw new Exception($"{arguments[0]}: not found");
                    //Console.WriteLine($"{arguments[0]}: not found");
                }
            }
        }
        private static void pwd()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }
        private static void cd(string[] arguments)
        {
            try
            {
                if (arguments[0] == "~") { arguments[0] = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);}
                Directory.SetCurrentDirectory(arguments[0]);
            }
            catch (Exception)
            {
                throw new Exception($"cd: {arguments[0]}: No such file or directory");
                //Console.WriteLine($"cd: {arguments[0]}: No such file or directory");
            }
        }
        private static bool IsBuiltInShellCommand(string command)
        {
           return Enum.IsDefined(typeof(CommandType), command);
        }
        private static string GetExecutablePath(string command)
        {
            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            string[] paths = pathVariable != null ? pathVariable.Split(':') : Array.Empty<string>();
            foreach (string path in paths)
            {
                string executablePath = Path.Join(path, command);
                if (File.Exists(executablePath))
                {
                    return executablePath;
                }
            }
            return "";
        }

    }
}
