using PoultryFarmApi.Models;

namespace PoultryFarmApi.Interfaces
{
    public interface ICoopRepository
    {
        Task<Coop> GetByIdAsync(int id);
        Task<IEnumerable<Coop>> GetAllAsync();
        Task AddAsync(Coop coop);
        Task UpdateAsync(Coop coop);
        Task DeleteAsync(int id);
    }
}