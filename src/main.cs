using codecrafters_shell.src;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

while (true) {
    // Uncomment this line to pass the first stage
    Console.Write("$ ");

    // Wait for user input
    //Console.ReadLine();
    //string? commandLine = Console.ReadLine();
    StringBuilder userInput = new StringBuilder();
    bool running = true;
    while (running)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        switch (keyInfo.Key)
        {
            case ConsoleKey.Enter:
                running = false;
                Console.WriteLine();
                break;
            case ConsoleKey.Tab:
                if (userInput.Length != 0)
                {
                    AutoCompleter.Complete(userInput);
                }
                break;
            case ConsoleKey.Backspace:
                if (userInput.Length != 0)
                {
                    userInput.Remove(userInput.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else {Console.Write('\a'); }
                break;
            default:
                userInput.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
                break;
        }
    }
    string commandLine = userInput.ToString();
    string command;
    string[] arguments;
    string outputDestination = "";
    string errorDestination = "";
    try
    {
        Parser.Parse(commandLine, out command, out arguments, ref outputDestination, ref errorDestination);
        if (!string.IsNullOrEmpty(command))
        {
            TextWriter originalOutput = Console.Out;
            TextWriter originalError = Console.Error;
            using (TextWriter output = string.IsNullOrEmpty(outputDestination) ? Console.Out : new StreamWriter(outputDestination, append: true))
            using (TextWriter error = string.IsNullOrEmpty(errorDestination) ? Console.Error : new StreamWriter(errorDestination, append: true))
            {
                try
                {
                    Console.SetOut(output);
                    Console.SetError(error);
                    CommandHandler.RunShellOrExecutable(command, arguments);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
                finally
                {
                    Console.SetOut(originalOutput);
                    Console.SetError(originalError);
                }

            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
            
            









                /*if (!string.IsNullOrEmpty(outputDestination))
            {
                TextWriter originalConsole = Console.Out; 
                using (StreamWriter writer = new StreamWriter(outputDestination , append : true))
                {
                    try
                    {
                        Console.SetOut(writer);
                        CommandHandler.RunShellOrExecutable(command, arguments);
                    }
                    finally
                    {
                        Console.SetOut(originalConsole);
                    }
                }                
            }
            else
            {
                CommandHandler.RunShellOrExecutable(command, arguments);
            }*/
  /*      }
    }
    catch (Exception ex)
    {
        if (!string.IsNullOrEmpty(errorDestination))
        {
            File.AppendAllText(errorDestination, $"{ex.Message}{Environment.NewLine}");
        }
        else
        {
            Console.WriteLine(ex.Message);
        }
    }*/
   




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


