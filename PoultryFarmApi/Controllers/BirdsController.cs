using Microsoft.AspNetCore.Mvc;
using PoultryFarmApi.Models;
using PoultryFarmApi.Services;

namespace PoultryFarmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BirdsController : ControllerBase
    {
        private readonly BirdService _birdService;

        public BirdsController(BirdService birdService)
        {
            _birdService = birdService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBird([FromBody] Bird bird)
        {
            var createdBird = await _birdService.RegisterNewBirdAsync(bird);
            return CreatedAtAction(nameof(CreateBird), new { id = createdBird.Id }, createdBird);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var birds = await _birdService.GetAllBirdsAsync();
            return Ok(birds);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Bird bird)
        {
            await _birdService.UpdateBirdAsync(id, bird);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _birdService.DeleteBirdAsync(id);
            return NoContent();
        }
    }
}