using System;
using System.Collections.Generic;
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
                        goto default;
                    }
                case "echo":
                    echo(arguments);
                    break;
                case "type":
                    type(arguments);
                    break;
                default:
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
        static private void type(string[] arguments)
        {
            bool exists = Enum.IsDefined(typeof(CommandType), arguments[0]);
            if (exists)
            {
                Console.WriteLine($"{arguments[0]} is a shell builtin");

            }
            else 
            {
                Console.WriteLine($"{arguments[0]}: not found");
            }
        }



    }



}
