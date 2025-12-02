using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Models;

namespace PoultryFarmApi.Services
{
    public class CoopService
    {
        private readonly ICoopRepository _coopRepository;

        public CoopService(ICoopRepository coopRepository)
        {
            _coopRepository = coopRepository;
        }

        public async Task<IEnumerable<Coop>> GetAllCoopsAsync()
        {
            return await _coopRepository.GetAllAsync();
        }

        public async Task<Coop> GetCoopByIdAsync(int id)
        {
            var coop = await _coopRepository.GetByIdAsync(id);
            if (coop == null) throw new KeyNotFoundException("Coop not found");
            return coop;
        }

        public async Task CreateCoopAsync(Coop coop)
        {
            if (string.IsNullOrEmpty(coop.Name))
                throw new ArgumentException("Coop name is required");

            await _coopRepository.AddAsync(coop);
        }

        public async Task UpdateCoopAsync(int id, Coop coop)
        {
            var existing = await _coopRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Coop not found");

            existing.Name = coop.Name;
            existing.MaxCapacity = coop.MaxCapacity;

            await _coopRepository.UpdateAsync(existing);
        }

        public async Task DeleteCoopAsync(int id)
        {
            var existing = await _coopRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Coop not found");

            await _coopRepository.DeleteAsync(id);
        }
    }
}