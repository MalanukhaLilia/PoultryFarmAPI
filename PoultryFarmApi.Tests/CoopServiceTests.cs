using Moq;
using PoultryFarmApi.Models;
using PoultryFarmApi.Services;
using PoultryFarmApi.Interfaces;

namespace PoultryFarmApi.Tests
{
    public class CoopServiceTests
    {
        private CoopService CreateCoopService(Mock<ICoopRepository> mockRepo)
        {
            return new CoopService(mockRepo.Object);
        }

        [Fact]
        public async Task CreateCoop_ShouldCallAdd_WhenNameIsValid()
        {
            var mockRepo = new Mock<ICoopRepository>();
            var service = CreateCoopService(mockRepo);

            const string VALID_COOP_NAME = "Main Coop";
            const int MAX_CAPACITY = 100;

            var coop = new Coop { Name = VALID_COOP_NAME, MaxCapacity = MAX_CAPACITY };

            await service.CreateCoopAsync(coop);

            mockRepo.Verify(repo => repo.AddAsync(coop), Times.Once);
        }

        [Fact]
        public async Task CreateCoop_ShouldThrowException_WhenNameIsEmpty()
        {
            var mockRepo = new Mock<ICoopRepository>();
            var service = CreateCoopService(mockRepo);

            var invalidCoop = new Coop { Name = string.Empty, MaxCapacity = 10 };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateCoopAsync(invalidCoop));

            Assert.Equal("Coop name is required", exception.Message);
        }

        [Fact]
        public async Task GetCoopById_ShouldReturnCoop_WhenExists()
        {
            var mockRepo = new Mock<ICoopRepository>();
            var service = CreateCoopService(mockRepo);

            const int COOP_ID = 1;
            const string EXPECTED_NAME = "Test Coop";
            var expectedCoop = new Coop { Id = COOP_ID, Name = EXPECTED_NAME };

            mockRepo.Setup(repo => repo.GetByIdAsync(COOP_ID))
                    .ReturnsAsync(expectedCoop);

            var result = await service.GetCoopByIdAsync(COOP_ID);

            Assert.Equal(EXPECTED_NAME, result.Name);
        }

        [Fact]
        public async Task DeleteCoop_ShouldThrowException_WhenCoopNotFound()
        {
            var mockRepo = new Mock<ICoopRepository>();
            var service = CreateCoopService(mockRepo);

            const int NON_EXISTENT_ID = 55;

            mockRepo.Setup(repo => repo.GetByIdAsync(NON_EXISTENT_ID))
                    .ReturnsAsync((Coop)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.DeleteCoopAsync(NON_EXISTENT_ID));
        }

        [Fact]
        public async Task GetAllCoops_ShouldReturnCorrectCount()
        {
            var mockRepo = new Mock<ICoopRepository>();
            var service = CreateCoopService(mockRepo);

            var coopsList = new List<Coop> { new Coop(), new Coop() };

            mockRepo.Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(coopsList);

            var result = await service.GetAllCoopsAsync();

            Assert.Equal(2, result.Count());
        }
    }
}