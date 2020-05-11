using System.Threading.Tasks;

namespace JokeGenerator.Service.Name
{
    public interface INameService
    {
        Task<CharacterName> GetName();
    }
}
