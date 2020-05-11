using System;
using System.Diagnostics.CodeAnalysis;

namespace JokeGenerator.Prompt
{
    [ExcludeFromCodeCoverage]
    internal sealed class ConsolePrompt : IPrompt
    {
        bool IPrompt.Confirm(string message, char confirmationKey)
        {
            Console.WriteLine(message);
            Console.Write("> ");
            bool confirmed = Console.ReadKey().KeyChar == confirmationKey;
            Console.WriteLine();
            return confirmed;
        }

        string IPrompt.Input(string message)
        {
            Console.WriteLine(message);
            Console.Write("> ");
            return Console.ReadLine();
        }

        char IPrompt.InputKey(string message)
        {
            Console.WriteLine(message);
            Console.Write("> ");
            var key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return key;
        }
    }
}
