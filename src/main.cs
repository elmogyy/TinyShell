using codecrafters_shell.src;
using System;
using System.Net;
using System.Net.Sockets;

while (true) {
    // Uncomment this line to pass the first stage
    Console.Write("$ ");

    // Wait for user input
    //Console.ReadLine();
    string? user_input = Console.ReadLine();
    string command;
    string[] arguments;
    Parser.Parse(user_input, out command, out arguments);
    CommandHandler.RunShellOrExecutable(command, arguments);




    /*string? command = Console.ReadLine();
    if (command != null)
    {
        string[] cmdArgs = command.Split(' ');
        switch (cmdArgs[0])
        {
            case "exit":
                {
                    if(cmdArgs[cmdArgs.Length-1] == "0") { Environment.Exit(0);}
                    goto default;
                }
            case "echo":
                {
                    for(int i = 1;i< cmdArgs.Length; i++)
                    {
                        Console.Write(cmdArgs[i]+" ");
                    }
                    Console.WriteLine();
                    break;
                }

            default:
                {
                    Console.WriteLine($"{command}: command not found");
                    break;
                }
        }
    }*/
    /* if (command == "") { Environment.Exit(0); }
     if (command.StartsWith("exit"))
     {
         var cmdArgs = command.Split(' ');
         Environment.Exit(int.Parse(cmdArgs[1]));
     }
     else if (command.StartsWith("echo"))
     {

     }
     Console.WriteLine($"{command}: command not found");*/

}
