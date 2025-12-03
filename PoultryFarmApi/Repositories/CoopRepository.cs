using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Models;

namespace PoultryFarmApi.Repositories
{
    public class CoopRepository : ICoopRepository
    {
        private readonly List<Coop> _coops = new List<Coop>();
        private int _nextId = 1;

        public async Task<Coop> GetByIdAsync(int id)
        {
            var coop = _coops.FirstOrDefault(c => c.Id == id);
            return await Task.FromResult(coop);
        }

        public async Task<IEnumerable<Coop>> GetAllAsync()
        {
            return await Task.FromResult(_coops);
        }

        public async Task AddAsync(Coop coop)
        {
            coop.Id = _nextId++;
            if (coop.Birds == null)
            {
                coop.Birds = new List<Bird>();
            }
            _coops.Add(coop);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Coop coop)
        {
            var index = _coops.FindIndex(c => c.Id == coop.Id);
            if (index != -1)
            {
                _coops[index] = coop;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var coop = _coops.FirstOrDefault(c => c.Id == id);
            if (coop != null)
            {
                _coops.Remove(coop);
            }
            await Task.CompletedTask;
        }
    }
}