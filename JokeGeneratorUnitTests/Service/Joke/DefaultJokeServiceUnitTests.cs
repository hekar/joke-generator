using System;
using System.Net.Http;
using Xunit;
using JokeGenerator.Service.Joke;
using Newtonsoft.Json;

namespace JokeGeneratorUnitTests.Service.Name
{
    public class DefaultJokeServiceUnitTests
    {
        private static readonly Uri RandomJokeUri = new Uri("https://api.chucknorris.io/jokes/random");
        private static readonly Joke DefaultJoke = new Joke("Some joke");

        private static readonly Uri CategoryUri = new Uri("https://api.chucknorris.io/jokes/categories");
        private static readonly string[] DefaultCategories = new[] { "category-1", "category-2", "category-3" };

        [Fact]
        public async void GetRandomJoke_Returns_ValidJoke()
        {
            // Given
            var messageHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(DefaultJoke));
            var httpClient = new HttpClient(messageHandler);
            IJokeService<CategoryQuery> sut = new DefaultJokeService(httpClient);

            // When
            var joke = await sut.GetRandomJoke(CategoryQuery.None);

            // Then
            Assert.Equal(RandomJokeUri, messageHandler.RequestMessage.RequestUri);
            Assert.Equal(DefaultJoke.Value, joke.Value);
        }

        [Fact]
        public async void GetRandomJoke_Returns_ValidJokeBasedOnCategory()
        {
            // Given
            var messageHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(DefaultJoke));
            var httpClient = new HttpClient(messageHandler);
            IJokeService<CategoryQuery> sut = new DefaultJokeService(httpClient);

            // When
            var joke = await sut.GetRandomJoke(new CategoryQuery("my-category"));

            // Then
            Assert.Equal(new Uri(RandomJokeUri, "?category=my-category"), messageHandler.RequestMessage.RequestUri);
            Assert.Equal(DefaultJoke.Value, joke.Value);
        }

        [Fact]
        public async void GetRandomJoke_Throws_HttpStatusException()
        {
            // Given
            var messageHandler = new FakeHttpMessageHandler(new HttpRequestException());
            var httpClient = new HttpClient(messageHandler);
            IJokeService<CategoryQuery> sut = new DefaultJokeService(httpClient);

            // When
            var recorded = await Record.ExceptionAsync(() => sut.GetRandomJoke(CategoryQuery.None));

            // Then
            Assert.IsType<HttpRequestException>(recorded);
        }

        [Fact]
        public async void GetCategories_Returns_ValidCategories()
        {
            // Given
            var messageHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(DefaultCategories));
            var httpClient = new HttpClient(messageHandler);
            IJokeService<CategoryQuery> sut = new DefaultJokeService(httpClient);

            // When
            var categories = await sut.GetCategories();

            // Then
            Assert.Equal(CategoryUri, messageHandler.RequestMessage.RequestUri);
            Assert.Equal(DefaultCategories, categories);
        }

        [Fact]
        public async void GetCategories_Throws_HttpStatusException()
        {
            // Given
            var messageHandler = new FakeHttpMessageHandler(new HttpRequestException());
            var httpClient = new HttpClient(messageHandler);
            IJokeService<CategoryQuery> sut = new DefaultJokeService(httpClient);

            // When
            var recorded = await Record.ExceptionAsync(() => sut.GetCategories());

            // Then
            Assert.IsType<HttpRequestException>(recorded);
        }
    }
}
