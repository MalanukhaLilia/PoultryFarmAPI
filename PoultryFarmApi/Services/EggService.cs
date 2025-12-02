using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Models;

namespace PoultryFarmApi.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }

    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.Now;
    }

    public class EggService
    {
        private readonly IEggRepository _eggRepository;
        private readonly ICoopRepository _coopRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public EggService(IEggRepository eggRepository, ICoopRepository coopRepository)
            : this(eggRepository, coopRepository, new SystemDateTimeProvider())
        {
        }

        public EggService(IEggRepository eggRepository, ICoopRepository coopRepository, IDateTimeProvider dateTimeProvider)
        {
            _eggRepository = eggRepository;
            _coopRepository = coopRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task RecordProductionAsync(EggProduction production)
        {
            if (production.EggCount < 0 || production.BrokenCount < 0)
            {
                throw new ArgumentException("Egg count cannot be negative.");
            }

            if (production.BrokenCount > production.EggCount)
            {
                throw new ArgumentException("Broken eggs cannot exceed total eggs.");
            }

            if (production.CollectionDate > _dateTimeProvider.Now())
            {
                throw new InvalidOperationException("Cannot record production in the future.");
            }

            var coop = await _coopRepository.GetByIdAsync(production.CoopId);
            if (coop == null)
            {
                throw new KeyNotFoundException($"Coop with ID {production.CoopId} not found.");
            }

            await _eggRepository.AddAsync(production);
        }

        public async Task<int> GetTotalGoodEggsForCoopAsync(int coopId)
        {
            var productions = await _eggRepository.GetByCoopIdAsync(coopId);

            int totalGoodEggs = 0;
            foreach (var record in productions)
            {
                totalGoodEggs += (record.EggCount - record.BrokenCount);
            }

            return totalGoodEggs;
        }
    }
}