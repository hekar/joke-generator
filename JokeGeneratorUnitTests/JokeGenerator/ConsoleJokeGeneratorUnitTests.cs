using System.Threading.Tasks;
using JokeGenerator;
using JokeGenerator.JokeGenerator;
using JokeGenerator.Prompt;
using JokeGenerator.Service.Joke;
using JokeGenerator.Service.Name;
using NSubstitute;
using Xunit;

namespace JokeGeneratorUnitTests.Service.Name
{
    public class ConsoleJokeGeneratorUnitTests
    {
        private static readonly string[] DefaultCategories = new[] { "category-1", "category-2" };

        private static readonly CharacterName DefaultCharacterName = new CharacterName()
        {
            Name = "Default",
            Surname = "Last",
        };

        private static readonly CharacterName ReplacedCharacterName = new CharacterName()
        {
            Name = "Replaced",
            Surname = "WithNewName",
        };

        private static readonly string DefaultJokeValue = "Some Joke";

        [Fact]
        public async void ProcessEvent_QuitKey_ReturnsFalse()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(Arg.Any<string>()).Returns('q');
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.False(proceed);
        }

        [Fact]
        public async void ProcessEvent_Question_PrintsUsage()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(Arg.Any<string>()).Returns('?');
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.True(proceed);
            printer.Received(1).WriteLine("Press ? to get instructions");
            printer.Received(1).WriteLine("Press c to get categories");
            printer.Received(1).WriteLine("Press r to get random jokes");
            printer.Received(1).Write("Press q to quit");
        }

        [Fact]
        public async void ProcessEvent_Categories_DisplaysCategories()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(Arg.Any<string>()).Returns('c');
            jokeService.GetCategories().Returns(Task.FromResult(DefaultCategories));
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.True(proceed);
            await jokeService.Received(1).GetCategories();
            printer.Received(1).WriteLine(DefaultCategories[0]);
            printer.Received(1).WriteLine(DefaultCategories[1]);
        }

        [Fact]
        public async void ProcessEvent_SelectJoke_RandomName()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(string.Empty).Returns('r');
            prompt.Confirm("Want to use a random name? y/(n)").Returns(true);
            prompt.Confirm("Want to specify a category? y/(n)").Returns(false);
            prompt.Input("How many jokes do you want? (1-9)").Returns("1");
            nameService.GetName().Returns(Task.FromResult(ReplacedCharacterName));
            jokeService.GetRandomJoke(Arg.Any<CategoryQuery>())
                .Returns(new Joke($"{DefaultCharacterName.ToString()} Joke", DefaultCharacterName.ToString()));
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.True(proceed);
            await nameService.Received(1).GetName();
            printer.Received(0).WriteLine($"{DefaultCharacterName.ToString()} Joke");
            printer.Received(1).WriteLine($"{ReplacedCharacterName.ToString()} Joke");
        }


        [Fact]
        public async void ProcessEvent_SelectJoke_RandomNameNotCalled()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(string.Empty).Returns('r');
            prompt.Confirm("Want to use a random name? y/(n)").Returns(false);
            prompt.Confirm("Want to specify a category? y/(n)").Returns(false);
            prompt.Input("How many jokes do you want? (1-9)").Returns("1");
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.True(proceed);
            await nameService.Received(0).GetName();
        }


        [Fact]
        public async void ProcessEvent_SelectJoke_CategoryQuery()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(string.Empty).Returns('r');
            prompt.Confirm("Want to use a random name? y/(n)").Returns(false);
            prompt.Confirm("Want to specify a category? y/(n)").Returns(true);
            prompt.Input("Enter a category:").Returns(DefaultCategories[0]);
            prompt.Input("How many jokes do you want? (1-9)").Returns("1");
            var categoryQuery = new CategoryQuery(DefaultCategories[0]);
            jokeService.GetRandomJoke(categoryQuery).Returns(new Joke(DefaultJokeValue));
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.True(proceed);
            jokeService.Received(1).GetRandomJoke(categoryQuery);
            printer.Received(1).WriteLine(DefaultJokeValue);
        }

        [Fact]
        public async void ProcessEvent_SelectJoke_LoopUntilValidInputCount()
        {
            // Given
            var jokeService = Substitute.For<IJokeService<CategoryQuery>>();
            var nameService = Substitute.For<INameService>();
            var prompt = Substitute.For<IPrompt>();
            var printer = Substitute.For<IPrinter>();

            prompt.InputKey(string.Empty).Returns('r');
            prompt.Confirm("Want to use a random name? y/(n)").Returns(false);
            prompt.Confirm("Want to specify a category? y/(n)").Returns(true);
            prompt.Input("Enter a category:").Returns(DefaultCategories[0]);
            prompt.Input("How many jokes do you want? (1-9)").Returns(
                "0", "-1", "-2", "10", "9"
            );
            var categoryQuery = new CategoryQuery(DefaultCategories[0]);
            jokeService.GetRandomJoke(categoryQuery).Returns(new Joke(DefaultJokeValue));
            var sut = new ConsoleJokeGenerator(jokeService, nameService, prompt, printer);

            // When
            var proceed = await sut.ProcessEvent();

            // Then
            Assert.True(proceed);
            jokeService.Received(9).GetRandomJoke(categoryQuery);
            printer.Received(1).WriteLine(string.Empty);
            printer.Received(9).WriteLine(DefaultJokeValue);
            printer.Received(4).WriteLine($"Please enter a count between {ConsoleJokeGenerator.MinJokeCount} and {ConsoleJokeGenerator.MaxJokeCount}");
            prompt.Received(0).Input(DefaultJokeValue);
        }
    }
}
