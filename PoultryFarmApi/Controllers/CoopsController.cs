using Microsoft.AspNetCore.Mvc;
using PoultryFarmApi.Models;
using PoultryFarmApi.Services;

namespace PoultryFarmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoopsController : ControllerBase
    {
        private readonly CoopService _coopService;

        public CoopsController(CoopService coopService)
        {
            _coopService = coopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _coopService.GetAllCoopsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _coopService.GetCoopByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Coop coop)
        {
            await _coopService.CreateCoopAsync(coop);
            return CreatedAtAction(nameof(GetById), new { id = coop.Id }, coop);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Coop coop)
        {
            await _coopService.UpdateCoopAsync(id, coop);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _coopService.DeleteCoopAsync(id);
            return NoContent();
        }
    }
}