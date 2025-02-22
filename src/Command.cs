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
        type
    }
    readonly struct CommandData
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
    }


    static class CommandHandler
    {
        static public void Execute(string command, string[] arguments)
        {
            switch (command)
            {
                case "exit":
                    if (arguments.Length == 1 && arguments[0] == "0")
                    {
                        Environment.Exit(0);
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"{command}: command not found");
                        break;
                    }
                case "echo":
                    echo(arguments);
                    break;
                case "type":
                    type(arguments);
                    break;
                case "custom_exe_1234":
                    string exe = type(new string[] { command });
                    if (exe != "")
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = exe;
                        process.StartInfo.Arguments = string.Join(" ", arguments);
                        process.Start();
                    }
                    break;

                default:
                    /*string exe = type(new string[] { command });
                    if ( exe != "")
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = exe;
                        process.StartInfo.Arguments = string.Join(" ", arguments);
                        process.Start();
                        break;

                    }*/
                    Console.WriteLine($"{command}: command not found");
                    break;        
            }
        }

        static private void echo(string[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == "") Console.Write(" ");
                else { Console.Write($"{arguments[i]} "); }
            }
            Console.WriteLine();
        }
        static private string type(string[] arguments)
        {
            bool exists = Enum.IsDefined(typeof(CommandType), arguments[0]);
            if (exists)
            {
                Console.WriteLine($"{arguments[0]} is a shell builtin");
                return arguments[0];

            }
            else 
            {
                string? pathVariable = Environment.GetEnvironmentVariable("PATH");
                string[] paths = pathVariable!= null ? pathVariable.Split(':'): Array.Empty<string>();
                foreach (string path in paths)
                {
                    string exe_path = Path.Join(path, arguments[0]);
                    if (File.Exists(exe_path))
                    {
                        Console.WriteLine($"{arguments[0]} is {exe_path}");
                        return exe_path;
                    }
                }
                Console.WriteLine($"{arguments[0]}: not found");
                return "";
            }
        }



    }



}
