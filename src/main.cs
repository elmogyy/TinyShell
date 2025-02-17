using System.Net;
using System.Net.Sockets;

while (true) {
    // Uncomment this line to pass the first stage
    Console.Write("$ ");

    // Wait for user input
    //Console.ReadLine();

    //Handle invalid commands
    string? command = Console.ReadLine();
    if(command == "exit 0") { Environment.Exit(0); }
    /*if (command.StartsWith("exit"))
    {
        var cmdArgs = command.Split(' ');
        Environment.Exit(int.Parse(cmdArgs[1]));
    }*/
    Console.WriteLine($"{command}: command not found");
   
}