using Moq;
using PoultryFarmApi.Models;
using PoultryFarmApi.Services;
using PoultryFarmApi.Interfaces;

namespace PoultryFarmApi.Tests
{
    public class BirdServiceTests
    {
        private BirdService CreateBirdService(Mock<IBirdRepository> birdRepo, Mock<ICoopRepository> coopRepo)
        {
            return new BirdService(birdRepo.Object, coopRepo.Object);
        }

        [Fact]
        public async Task RegisterNewBird_ShouldReturnAddedBird_WhenCoopHasSpace()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const int COOP_ID = 1;
            const double BIRD_WEIGHT = 1.5;
            const string BIRD_SPECIES = "Chicken";

            var coop = new Coop
            {
                Id = COOP_ID,
                MaxCapacity = 5,
                Birds = new List<Bird> { new Bird(), new Bird() }
            };

            var newBird = new Bird { CoopId = COOP_ID, Weight = BIRD_WEIGHT, Species = BIRD_SPECIES };

            mockCoopRepo.Setup(repo => repo.GetByIdAsync(COOP_ID)).ReturnsAsync(coop);
            mockBirdRepo.Setup(repo => repo.AddAsync(newBird)).ReturnsAsync(newBird);

            var result = await service.RegisterNewBirdAsync(newBird);

            Assert.Equal(BIRD_SPECIES, result.Species);
        }

        [Fact]
        public async Task RegisterNewBird_ShouldCallRepository_WhenCoopHasSpace()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const int COOP_ID = 1;
            var coop = new Coop { Id = COOP_ID, MaxCapacity = 10, Birds = new List<Bird>() };
            var newBird = new Bird { CoopId = COOP_ID, Weight = 1.5 };

            mockCoopRepo.Setup(repo => repo.GetByIdAsync(COOP_ID)).ReturnsAsync(coop);

            await service.RegisterNewBirdAsync(newBird);

            mockBirdRepo.Verify(repo => repo.AddAsync(newBird), Times.Once);
        }

        [Fact]
        public async Task RegisterNewBird_ShouldThrowException_WhenCoopIsFull()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const int COOP_ID = 2;
            var fullCoop = new Coop
            {
                Id = COOP_ID,
                MaxCapacity = 2,
                Birds = new List<Bird> { new Bird(), new Bird() }
            };

            var newBird = new Bird { CoopId = COOP_ID, Weight = 1.2 };

            mockCoopRepo.Setup(repo => repo.GetByIdAsync(COOP_ID)).ReturnsAsync(fullCoop);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.RegisterNewBirdAsync(newBird));

            Assert.Equal("Cannot add bird. The coop is fully occupied.", exception.Message);
        }

        [Fact]
        public async Task RegisterNewBird_ShouldThrowException_WhenWeightIsNegative()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const double NEGATIVE_WEIGHT = -5;
            var bird = new Bird { Weight = NEGATIVE_WEIGHT, Species = "Duck" };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                service.RegisterNewBirdAsync(bird));

            Assert.Equal("Bird weight must be positive.", exception.Message);
        }

        [Fact]
        public async Task RegisterNewBird_ShouldThrowException_WhenCoopDoesNotExist()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const int INVALID_COOP_ID = 999;
            var bird = new Bird { CoopId = INVALID_COOP_ID, Weight = 2.5 };

            mockCoopRepo.Setup(repo => repo.GetByIdAsync(INVALID_COOP_ID))
                        .ReturnsAsync((Coop)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.RegisterNewBirdAsync(bird));

            Assert.Equal($"Coop with ID {INVALID_COOP_ID} not found.", exception.Message);
        }

        [Fact]
        public async Task GetAllBirds_ShouldReturnListOfBirds()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            var birdsList = new List<Bird>
            {
                new Bird { Id = 1, Species = "Chicken" },
                new Bird { Id = 2, Species = "Goose" }
            };

            mockBirdRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(birdsList);

            var result = await service.GetAllBirdsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateBird_ShouldCallRepositoryUpdate_WhenBirdExists()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const int BIRD_ID = 10;
            const double OLD_WEIGHT = 2.0;
            const double NEW_WEIGHT = 3.5;

            var existingBird = new Bird { Id = BIRD_ID, Weight = OLD_WEIGHT, CoopId = 1 };
            var updatedData = new Bird { Id = BIRD_ID, Weight = NEW_WEIGHT, CoopId = 1 };

            mockBirdRepo.Setup(repo => repo.GetByIdAsync(BIRD_ID)).ReturnsAsync(existingBird);

            await service.UpdateBirdAsync(BIRD_ID, updatedData);

            Assert.Equal(NEW_WEIGHT, existingBird.Weight);
        }

        [Fact]
        public async Task DeleteBird_ShouldCallRepositoryDelete_WhenBirdExists()
        {
            var mockBirdRepo = new Mock<IBirdRepository>();
            var mockCoopRepo = new Mock<ICoopRepository>();
            var service = CreateBirdService(mockBirdRepo, mockCoopRepo);

            const int BIRD_ID = 5;
            var existingBird = new Bird { Id = BIRD_ID };

            mockBirdRepo.Setup(repo => repo.GetByIdAsync(BIRD_ID)).ReturnsAsync(existingBird);

            await service.DeleteBirdAsync(BIRD_ID);

            mockBirdRepo.Verify(repo => repo.DeleteAsync(BIRD_ID), Times.Once);
        }
    }
}