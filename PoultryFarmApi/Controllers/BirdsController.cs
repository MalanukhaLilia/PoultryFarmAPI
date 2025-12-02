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
            try
            {
                var createdBird = await _birdService.RegisterNewBirdAsync(bird);
                return Ok(createdBird);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
            try
            {
                await _birdService.UpdateBirdAsync(id, bird);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _birdService.DeleteBirdAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}