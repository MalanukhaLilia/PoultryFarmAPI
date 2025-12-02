using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Models;

namespace PoultryFarmApi.Services
{
    public class BirdService
    {
        private readonly IBirdRepository _birdRepository;
        private readonly ICoopRepository _coopRepository;

        public BirdService(IBirdRepository birdRepository, ICoopRepository coopRepository)
        {
            _birdRepository = birdRepository;
            _coopRepository = coopRepository;
        }

        public async Task<Bird> RegisterNewBirdAsync(Bird bird)
        {
            if (bird.Weight <= 0)
            {
                throw new ArgumentException("Bird weight must be positive.");
            }

            var coop = await _coopRepository.GetByIdAsync(bird.CoopId);
            if (coop == null)
            {
                throw new KeyNotFoundException($"Coop with ID {bird.CoopId} not found.");
            }

            if (coop.Birds.Count >= coop.MaxCapacity)
            {
                throw new InvalidOperationException("Cannot add bird. The coop is fully occupied.");
            }

            return await _birdRepository.AddAsync(bird);
        }

        public async Task<IEnumerable<Bird>> GetAllBirdsAsync()
        {
            return await _birdRepository.GetAllAsync();
        }

        public async Task UpdateBirdAsync(int id, Bird updatedBird)
        {
            var existingBird = await _birdRepository.GetByIdAsync(id);
            if (existingBird == null)
            {
                throw new KeyNotFoundException($"Bird with ID {id} not found.");
            }

            existingBird.Weight = updatedBird.Weight;
            existingBird.AgeMonths = updatedBird.AgeMonths;

            if (existingBird.CoopId != updatedBird.CoopId && updatedBird.CoopId != 0)
            {
                existingBird.CoopId = updatedBird.CoopId;
            }

            await _birdRepository.UpdateAsync(existingBird);
        }

        public async Task DeleteBirdAsync(int id)
        {
            var existingBird = await _birdRepository.GetByIdAsync(id);
            if (existingBird == null)
            {
                throw new KeyNotFoundException($"Bird with ID {id} not found.");
            }

            await _birdRepository.DeleteAsync(id);
        }
    }
}