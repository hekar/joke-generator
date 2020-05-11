using System;
using System.Diagnostics.CodeAnalysis;

namespace JokeGenerator
{
    [ExcludeFromCodeCoverage]
    internal sealed class ConsolePrinter : IPrinter
    {
        /// <inheritdoc/>
        IPrinter IPrinter.Write(string msg)
        {
            Console.Write(msg);
            return this;
        }

        /// <inheritdoc/>
        IPrinter IPrinter.WriteLine(string msg)
        {
            Console.WriteLine(msg);
            return this;
        }

        /// <inheritdoc/>
        IPrinter IPrinter.WriteLine(string msg, object[] args)
        {
            Console.WriteLine(msg, args);
            return this;
        }
    }
}
