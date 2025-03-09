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
