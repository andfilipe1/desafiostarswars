using StarWars.Model;
using System.Threading.Tasks;

namespace StarWarsApi.Util
{
    public interface IUtilHttpClient
    {
        Task<string> GetPlanets();
        Task<PlanetSWResult> GetPlanetByName(string name);
        Task<string> GetPlanetById(int id);
    }
}
