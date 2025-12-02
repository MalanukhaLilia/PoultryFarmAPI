using PoultryFarmApi.Models;

namespace PoultryFarmApi.Interfaces
{
    public interface IBirdRepository
    {
        Task<Bird> AddAsync(Bird bird);
        Task<IEnumerable<Bird>> GetAllAsync();
        Task<Bird> GetByIdAsync(int id);
        Task UpdateAsync(Bird bird);
        Task DeleteAsync(int id);
    }
}