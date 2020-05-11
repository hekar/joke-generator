using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using JokeGenerator.JokeGenerator;
using JokeGenerator.Service.Joke;
using JokeGenerator.Service.Name;
using JokeGenerator.Prompt;

namespace JokeGenerator
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private async static Task MainAsync()
        {
            var client = new HttpClient();
            var jokeFeed = new DefaultJokeService(client);
            var nameGenerator = new RandomNameService(client);
            var prompt = new ConsolePrompt();
            var printer = new ConsolePrinter();
            var generator = new ConsoleJokeGenerator(jokeFeed, nameGenerator, prompt, printer);
            await generator.EventLoop();
        }
    }
}
