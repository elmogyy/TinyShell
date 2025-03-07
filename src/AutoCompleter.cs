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
                    userInput.Append(" ");
                    Console.Write(" ");
                    break;
                case "ech":
                    userInput.Append("o ");
                    Console.Write("o ");
                    break;
                case "exit":
                    userInput.Append(" ");
                    Console.Write(" ");
                    break;
                case "exi":
                    userInput.Append("t ");
                    Console.Write("t ");
                    break;
                case "type":
                    userInput.Append(" ");
                    Console.Write(" ");
                    break;
                case "typ":
                    userInput.Append("e ");
                    Console.Write("e ");
                    break;
                default:
                    Console.Write("\a");
                    break;
            }
        }
        static public void Complete(StringBuilder userInput)
        {
            BuiltinCompletion(userInput);
        }
    }
}
