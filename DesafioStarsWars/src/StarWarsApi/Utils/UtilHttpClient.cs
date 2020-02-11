using StarWars.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StarWarsApi.Util
{
    public class UtilHttpClient : IUtilHttpClient
    {
        private string baseUrl => "https://swapi.co/api/";
        private void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private async Task<PlanetSW> GetPlanetPaginated(HttpClient client, int page)
        {
            PlanetSW planetRoot = null;
            HttpResponseMessage response = await client.GetAsync($"planets/?page={page}");

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                planetRoot = JsonConvert.DeserializeObject<PlanetSW>(result);
            }

            return planetRoot;

        }
        public async Task<string> GetPlanets()
        {
            using (HttpClient client = new HttpClient())
            {
                ConfigureHttpClient(client);

                HttpResponseMessage response = await client.GetAsync("planets");

                string result = string.Empty;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }

                return result;
            }
        }
        public async Task<PlanetSWResult> GetPlanetByName(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                ConfigureHttpClient(client);

                PlanetSWResult planetResult = null;

                int page = 1;

                do
                {
                    PlanetSW planetRoot = await GetPlanetPaginated(client, page);
                    if (planetRoot.next == null)
                        page = 0;

                    planetResult = planetRoot.results.FirstOrDefault(w => w.name.ToLower() == name.ToLower());
                    if (planetResult == null)
                        page++;

                } while (page > 0 && planetResult == null);

                return planetResult;
            }
        }
        public async Task<string> GetPlanetById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                ConfigureHttpClient(client);

                HttpResponseMessage response = await client.GetAsync("planets/" + id);

                string result = string.Empty;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }

                return result;
            }
        }
    }
}
