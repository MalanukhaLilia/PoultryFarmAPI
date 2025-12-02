using PoultryFarmApi.Models;

namespace PoultryFarmApi.Interfaces
{
    public interface IEggRepository
    {
        Task AddAsync(EggProduction production);
        Task<IEnumerable<EggProduction>> GetByCoopIdAsync(int coopId);
    }
}