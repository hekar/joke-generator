using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JokeGenerator.Service.Name
{
    internal sealed class RandomNameService : INameService
    {
        private const string Endpoint = "https://names.privserv.com/api/";
        private readonly HttpClient httpClient;

        public RandomNameService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        async Task<CharacterName> INameService.GetName()
        {
            var json = await this.httpClient.GetStringAsync(Endpoint);
            return JsonConvert.DeserializeObject<CharacterName>(json);
        }
    }
}
