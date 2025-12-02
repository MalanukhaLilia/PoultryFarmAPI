using Moq;
using PoultryFarmApi.Models;
using PoultryFarmApi.Services;
using PoultryFarmApi.Interfaces;

namespace PoultryFarmApi.Tests
{
    public class EggServiceTests
    {
        private EggService CreateEggService(Mock<IEggRepository> eggRepo, Mock<ICoopRepository> coopRepo, Mock<IDateTimeProvider> dateRepo)
        {
            return new EggService(eggRepo.Object, coopRepo.Object, dateRepo.Object);
        }

        [Fact]
        public async Task RecordProduction_ShouldThrowError_WhenBrokenExceedsTotal()
        {
            var mockEggRepo = new Mock<IEggRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var mockDate = new Mock<IDateTimeProvider>();
            var service = CreateEggService(mockEggRepo, mockCoopRepo, mockDate);

            const int TOTAL_EGGS = 10;
            const int BROKEN_EGGS = 15;

            var production = new EggProduction
            {
                EggCount = TOTAL_EGGS,
                BrokenCount = BROKEN_EGGS
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                service.RecordProductionAsync(production));

            Assert.Equal("Broken eggs cannot exceed total eggs.", exception.Message);
        }

        [Fact]
        public async Task GetTotalGoodEggs_ShouldCalculateCorrectly()
        {
            var mockEggRepo = new Mock<IEggRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var mockDate = new Mock<IDateTimeProvider>();
            var service = CreateEggService(mockEggRepo, mockCoopRepo, mockDate);

            const int COOP_ID = 1;
            var mockData = new List<EggProduction>
            {
                new EggProduction { EggCount = 100, BrokenCount = 5 },
                new EggProduction { EggCount = 50, BrokenCount = 0 }
            };

            mockEggRepo.Setup(repo => repo.GetByCoopIdAsync(COOP_ID))
                        .ReturnsAsync(mockData);

            var result = await service.GetTotalGoodEggsForCoopAsync(COOP_ID);

            Assert.Equal(145, result);
        }

        [Fact]
        public async Task RecordProduction_ShouldThrowException_WhenDateIsFuture()
        {
            var mockEggRepo = new Mock<IEggRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var mockDateTime = new Mock<IDateTimeProvider>();

            var fixedDate = new DateTime(2025, 1, 1);
            mockDateTime.Setup(d => d.Now()).Returns(fixedDate);

            const int COOP_ID = 1;
            mockCoopRepo.Setup(repo => repo.GetByIdAsync(COOP_ID))
                        .ReturnsAsync(new Coop { Id = COOP_ID });

            var service = CreateEggService(mockEggRepo, mockCoopRepo, mockDateTime);

            var production = new EggProduction
            {
                CoopId = COOP_ID,
                EggCount = 10,
                CollectionDate = fixedDate.AddDays(1)
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.RecordProductionAsync(production));

            Assert.Equal("Cannot record production in the future.", exception.Message);
        }

        [Fact]
        public async Task RecordProduction_ShouldThrowException_WhenEggCountIsNegative()
        {
            var mockEggRepo = new Mock<IEggRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var mockDate = new Mock<IDateTimeProvider>();
            var service = CreateEggService(mockEggRepo, mockCoopRepo, mockDate);

            const int NEGATIVE_COUNT = -10;
            var production = new EggProduction { EggCount = NEGATIVE_COUNT };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                service.RecordProductionAsync(production));

            Assert.Equal("Egg count cannot be negative.", exception.Message);
        }

        [Fact]
        public async Task RecordProduction_ShouldThrowException_WhenCoopNotFound()
        {
            var mockEggRepo = new Mock<IEggRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var mockDate = new Mock<IDateTimeProvider>();
            var service = CreateEggService(mockEggRepo, mockCoopRepo, mockDate);

            const int NON_EXISTENT_ID = 100;
            var production = new EggProduction { CoopId = NON_EXISTENT_ID, EggCount = 50 };

            mockCoopRepo.Setup(repo => repo.GetByIdAsync(NON_EXISTENT_ID))
                         .ReturnsAsync((Coop)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.RecordProductionAsync(production));

            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public async Task GetTotalGoodEggs_ShouldReturnZero_WhenNoRecordsExist()
        {
            var mockEggRepo = new Mock<IEggRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var mockDate = new Mock<IDateTimeProvider>();
            var service = CreateEggService(mockEggRepo, mockCoopRepo, mockDate);

            const int COOP_ID = 1;
            mockEggRepo.Setup(repo => repo.GetByCoopIdAsync(COOP_ID))
                        .ReturnsAsync(new List<EggProduction>());

            var result = await service.GetTotalGoodEggsForCoopAsync(COOP_ID);

            Assert.Equal(0, result);
        }
    }
}