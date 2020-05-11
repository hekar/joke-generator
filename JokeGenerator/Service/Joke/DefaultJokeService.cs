using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JokeGenerator.Service.Joke
{
    internal sealed class DefaultJokeService : IJokeService<CategoryQuery>
    {
        private const string Endpoint = "https://api.chucknorris.io";
        private const string DefaultName = "Chuck Norris";

        private readonly HttpClient httpClient;

        public DefaultJokeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        async Task<Joke> IJokeService<CategoryQuery>.GetRandomJoke(CategoryQuery query)
        {
            string url = $"{Endpoint}/jokes/random";
            if (query != CategoryQuery.None)
            {
                url += $"?category={query.Category}";
            }

            string json = await this.httpClient.GetStringAsync(url);
            var joke = JsonConvert.DeserializeObject<Joke>(json);
            joke.DefaultName = DefaultName;
            return joke;
        }

        async Task<string[]> IJokeService<CategoryQuery>.GetCategories()
        {
            var url = $"{Endpoint}/jokes/categories";

            string json = await this.httpClient.GetStringAsync(url);
            string[] categories = JsonConvert.DeserializeObject<String[]>(json);

            // TODO: Return IEnumerable<T> instead of string[]
            return categories;
        }
    }
}
