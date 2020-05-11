using System.Threading.Tasks;

namespace JokeGenerator.Service.Joke
{
    public interface IJokeService<T>
    {
        Task<Joke> GetRandomJoke(T query);
        Task<string[]> GetCategories();
    }
}
