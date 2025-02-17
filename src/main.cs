using System.Net;
using System.Net.Sockets;

while (true) {
    // Uncomment this line to pass the first stage
    Console.Write("$ ");

    // Wait for user input
    //Console.ReadLine();

    //Handle invalid commands
    string? Command = Console.ReadLine();
    Console.WriteLine($"{Command}: command not found");


}