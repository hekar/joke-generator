using System;
using System.Net.Http;
using System.Threading.Tasks;
using JokeGenerator.Prompt;
using JokeGenerator.Service.Joke;
using JokeGenerator.Service.Name;

namespace JokeGenerator.JokeGenerator
{
    internal sealed class ConsoleJokeGenerator
    {
        internal const int MinJokeCount = 1;
        internal const int MaxJokeCount = 9;

        private readonly IJokeService<CategoryQuery> jokeService;
        private readonly INameService nameService;
        private readonly IPrinter printer;
        private readonly IPrompt prompt;

        public ConsoleJokeGenerator(IJokeService<CategoryQuery> jokeService,
                                    INameService nameService,
                                    IPrompt prompt,
                                    IPrinter printer)
        {
            this.jokeService = jokeService;
            this.nameService = nameService;
            this.prompt = prompt;
            this.printer = printer;
        }

        public async Task EventLoop()
        {
            this.printer.Write("Press ? to get instructions");
            while (true)
            {
                if (!(await this.ProcessEvent()))
                {
                    break;
                }
            }
        }

        public async Task<bool> ProcessEvent()
        {
            var key = this.prompt.InputKey(string.Empty);
            this.printer.WriteLine(string.Empty);
            if (key == '?')
            {
                this.printer.WriteLine("Press ? to get instructions");
                this.printer.WriteLine("Press c to get categories");
                this.printer.WriteLine("Press r to get random jokes");
                this.printer.Write("Press q to quit");
            }
            else if (key == 'c')
            {
                try
                {
                    await DisplayCategories();
                }
                catch (HttpRequestException ex)
                {
                    this.printer.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    this.printer.WriteLine(ex.Message);
                }
            }
            else if (key == 'r')
            {
                try
                {
                    await SelectJoke();
                }
                catch (HttpRequestException ex)
                {
                    this.printer.WriteLine(ex.Message);
                    this.printer.WriteLine("Does the provided category exist?");
                }
                catch (Exception ex)
                {
                    this.printer.WriteLine(ex.Message);
                }
            }
            else if (key == 'q')
            {
                return false;
            }

            return true;
        }

        private async Task DisplayCategories()
        {
            var categories = await this.jokeService.GetCategories();
            foreach (var category in categories)
            {
                printer.WriteLine(category);
            }
        }

        private async Task SelectJoke()
        {
            CharacterName characterName = null;
            if (this.prompt.Confirm("Want to use a random name? y/(n)"))
            {
                characterName = await this.nameService.GetName();
            }

            CategoryQuery categoryQuery = CategoryQuery.None;
            if (this.prompt.Confirm("Want to specify a category? y/(n)"))
            {
                var category = this.prompt.Input("Enter a category:");
                categoryQuery = new CategoryQuery(category);
            }

            bool askForJokeCount = true;
            while (askForJokeCount)
            {
                if (int.TryParse(this.prompt.Input($"How many jokes do you want? ({MinJokeCount}-{MaxJokeCount})"), out int jokeCount))
                {
                    if (jokeCount < MinJokeCount || jokeCount > MaxJokeCount)
                    {
                        this.printer.WriteLine($"Please enter a count between {MinJokeCount} and {MaxJokeCount}");
                        continue;
                    }

                    for (int i = 0; i < jokeCount; i++)
                    {
                        var joke = await this.jokeService.GetRandomJoke(categoryQuery);
                        this.printer.WriteLine(this.FormatJoke(characterName, joke));
                    }
                    askForJokeCount = false;
                }
            }
        }

        private string FormatJoke(CharacterName characterName, Joke joke)
        {
            if (characterName != null)
            {
                return joke.ReplacedName(characterName.ToString());
            }
            else
            {
                return joke.Value;
            }
        }
    }
}
