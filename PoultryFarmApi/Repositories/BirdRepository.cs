using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Models;

namespace PoultryFarmApi.Repositories
{
    public class BirdRepository : IBirdRepository
    {
        private readonly List<Bird> _birds = new List<Bird>();
        private int _nextId = 1;

        public async Task<Bird> AddAsync(Bird bird)
        {
            bird.Id = _nextId++;
            _birds.Add(bird);
            return await Task.FromResult(bird);
        }

        public async Task<IEnumerable<Bird>> GetAllAsync()
        {
            return await Task.FromResult(_birds);
        }

        public async Task<Bird> GetByIdAsync(int id)
        {
            var bird = _birds.FirstOrDefault(b => b.Id == id);
            return await Task.FromResult(bird);
        }

        public async Task UpdateAsync(Bird bird)
        {
            var index = _birds.FindIndex(b => b.Id == bird.Id);
            if (index != -1)
            {
                _birds[index] = bird;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var bird = _birds.FirstOrDefault(b => b.Id == id);
            if (bird != null)
            {
                _birds.Remove(bird);
            }
            await Task.CompletedTask;
        }
    }
}