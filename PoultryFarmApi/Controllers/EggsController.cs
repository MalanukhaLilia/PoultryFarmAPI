using Microsoft.AspNetCore.Mvc;
using PoultryFarmApi.Models;
using PoultryFarmApi.Services;

namespace PoultryFarmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EggsController : ControllerBase
    {
        private readonly EggService _eggService;

        public EggsController(EggService eggService)
        {
            _eggService = eggService;
        }

        [HttpPost]
        public async Task<IActionResult> RecordEggs([FromBody] EggProduction production)
        {
            await _eggService.RecordProductionAsync(production);
            return Ok("Production recorded successfully.");
        }

        [HttpGet("stats/{id}")]
        public async Task<IActionResult> GetTotalGoodEggs(int id)
        {
            var total = await _eggService.GetTotalGoodEggsForCoopAsync(id);
            return Ok(new { CoopId = id, TotalGoodEggs = total });
        }
    }
}