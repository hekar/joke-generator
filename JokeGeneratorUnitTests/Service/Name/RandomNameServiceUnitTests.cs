using System;
using System.Net.Http;
using JokeGenerator.Service.Name;
using Newtonsoft.Json;
using Xunit;

namespace JokeGeneratorUnitTests.Service.Name
{
    public class RandomNameServiceUnitTests
    {
        private static readonly Uri DefaultUri = new Uri("https://names.privserv.com/api/");

        private static readonly CharacterName DefaultCharacterName = new CharacterName()
        {
            Name = "First",
            Surname = "Last"
        };


        [Fact]
        public async void GetName_Returns_CharacterName()
        {
            // Given
            var messageHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(DefaultCharacterName));
            var httpClient = new HttpClient(messageHandler);
            INameService sut = new RandomNameService(httpClient);

            // When
            var name = await sut.GetName();

            // Then
            Assert.Equal(DefaultUri, messageHandler.RequestMessage.RequestUri);
            Assert.Equal(DefaultCharacterName.Name, name.Name);
            Assert.Equal(DefaultCharacterName.Surname, name.Surname);
        }
    }
}
