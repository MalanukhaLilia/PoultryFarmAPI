using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Models;

namespace PoultryFarmApi.Repositories
{
    public class EggRepository : IEggRepository
    {
        private readonly List<EggProduction> _eggProductions = new List<EggProduction>();
        private int _nextId = 1;

        public async Task AddAsync(EggProduction production)
        {
            production.Id = _nextId++;
            _eggProductions.Add(production);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<EggProduction>> GetByCoopIdAsync(int coopId)
        {
            var result = _eggProductions.Where(e => e.CoopId == coopId);
            return await Task.FromResult(result);
        }
    }
}