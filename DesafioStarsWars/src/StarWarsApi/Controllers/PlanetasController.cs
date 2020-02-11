using StarWars.Model;
using StarWarsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace StarWarsApi.Controllers
{
    [Route("api/planetas")]
    [ApiController]
    public class PlanetasController : ControllerBase
    {
        private readonly IPlanetService planetService;

        public PlanetasController(IPlanetService planetService)
        {
            this.planetService = planetService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaneta([FromBody]Planeta planeta)
        {
            try
            {
                await planetService.AddPlanet(planeta);
                return new JsonResult(new { Message = "Planeta adicionado com sucesso." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = $"Erro: {ex.Message}." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterPlanetas()
        {
            var result = await planetService.GetPlanets();
            return new JsonResult(result);
        }

        [HttpGet("{nomePlaneta}")]
        public async Task<IActionResult> ObterPlanetaPorNome(string nomePlaneta)
        {
            object obj = await planetService.GetPlanetByName(nomePlaneta);

            if (obj != null)
                return new JsonResult(obj);

            return NotFound(new { Message = $"Não foi localizado um planeta com o nome {nomePlaneta}." });

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPlanetaPorId(int id)
        {
            object obj = await planetService.GetPlanetById(id);

            if (obj != null)
                return new JsonResult(obj);

            return NotFound(new { Message = $"Não foi localizado um planeta com o Id {id}." });

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoverPlaneta(int id)
        {
            bool result = await planetService.RemovePlanet(id);
            if (result)
                return new JsonResult(new { Message = "Planeta removido com sucesso." });

            return NotFound(new { Message = $"Não foi localizado um planeta com o Id {id}." });
        }
    }
}