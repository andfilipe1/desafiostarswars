using StarWars.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWarsApi.Services
{
    public interface IPlanetService
    {
        Task<List<object>> GetPlanets();

        Task<Planeta> AddPlanet(Planeta planet);
        Task<object> GetPlanetByName(string name);

        Task<object> GetPlanetById(int idPlanet);

        Task<bool> RemovePlanet(int idPlanet);
    }
}
