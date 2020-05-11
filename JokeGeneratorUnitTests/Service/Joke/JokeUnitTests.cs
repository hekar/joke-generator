using JokeGenerator.Service.Joke;
using Xunit;

namespace JokeGeneratorUnitTests.Service.Name
{
    public class JokeUnitTests
    {
        private const string DefaultName = "Billy James";
        private const string NewName = "New Name";

        [Fact]
        public void ReplacedWith_Should_ReplaceDefaultName()
        {
            // Given
            var sut = new Joke()
            {
                Value = $"{DefaultName} says hello",
                DefaultName = DefaultName,
            };

            // When
            var replaced = sut.ReplacedName(NewName);

            // Then
            Assert.Equal("New Name says hello", replaced);
        }

        [Fact]
        public void ReplacedWith_Should_NotReplaceName()
        {
            // Given
            var sut = new Joke()
            {
                Value = $"{DefaultName} says hello"
            };

            // When
            var replaced = sut.ReplacedName(NewName);

            // Then
            Assert.Equal($"{DefaultName} says hello", replaced);
        }
    }
}
