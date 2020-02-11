using StarWars.Infra.Interfaces;
using StarWars.Model;
using StarWarsApi.Util;
using StarWarsApi.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWarsApi.Services
{
    public class PlanetService : IPlanetService
    {
        private readonly IPlanetaRepository planetRepository;
        private readonly IUtilHttpClient utilHttpClient;
        private readonly IManageCache manageCache;

        private static object syncObject = Guid.NewGuid();

        public PlanetService(IPlanetaRepository planetRepository, IUtilHttpClient utilHttpClient, IManageCache manageCache)
        {
            this.planetRepository = planetRepository;
            this.utilHttpClient = utilHttpClient;
            this.manageCache = manageCache;
        }

        public async Task<Planeta> AddPlanet(Planeta planet)
        {
            return await planetRepository.AddAsync(planet);
        }

        public async Task<object> GetPlanetById(int idPlanet)
        {
            return await GetPlanet(idPlanet: idPlanet);
        }

        public async Task<object> GetPlanetByName(string name)
        {
            return await GetPlanet(name: name);
        }

        public async Task<List<object>> GetPlanets()
        {
            IList<Planeta> planets = await planetRepository.AllAsync();

            List<object> result = new List<object>();

            foreach (var planet in planets)
            {
                planet.TotalFilmes = GetTotalFilmsByPlanet(planet);
                result.Add(new { planet.Nome, planet.Clima, planet.Terreno, planet.TotalFilmes });
            }

            return result;
        }

        public async Task<bool> RemovePlanet(int idPlanet)
        {
            return await planetRepository.DeleteAsync(w => w.IdPlaneta == idPlanet);
        }

        private async Task<object> GetPlanet(string name = "", int idPlanet = 0)
        {

            Planeta planeta = await planetRepository.FindAsync(w => w.Nome == name || w.IdPlaneta == idPlanet);

            if (planeta == null) return null;

            planeta.TotalFilmes = GetTotalFilmsByPlanet(planeta);

            return new { planeta.Nome, planeta.Clima, planeta.Terreno, planeta.TotalFilmes };

        }

        private int GetTotalFilmsByPlanet(Planeta planeta)
        {
            lock (syncObject)
            {
                try
                {
                    string planetJson = manageCache.Cache.GetString(planeta.Nome);
                    if (string.IsNullOrEmpty(planetJson))
                    {
                        planeta.TotalFilmes = GetTotalFilmsInSWApi(planeta);

                        planetJson = JsonConvert.SerializeObject(planeta);
                        manageCache.Cache.SetString(planeta.Nome, planetJson, manageCache.SetCacheExpiration(TimeSpan.FromSeconds(20)));
                    }
                    else
                        planeta = JsonConvert.DeserializeObject<Planeta>(planetJson);

                    
                    return planeta.TotalFilmes;
                }
                catch
                {
                    return GetTotalFilmsInSWApi(planeta);
                }
            }
        }

        private int GetTotalFilmsInSWApi(Planeta planeta)
        {
            var p = utilHttpClient.GetPlanetByName(planeta.Nome).Result;
            return p.films.Count;
        }
    }
}
