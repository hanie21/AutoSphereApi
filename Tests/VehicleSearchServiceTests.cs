using AutoSphere.Api.Repositories;
using AutoSphere.Api.Services;
using Moq;
using Xunit;

namespace AutoSphere.Api.Tests.Services
{
    public class VehicleSearchServiceTests
    {
        private readonly Mock<IVehicleSearchRepository> _mockRepository;
        private readonly VehicleSearchService _service;
         private readonly Mock<ILogger<VehicleSearchService>> _mockLogger;

        public VehicleSearchServiceTests()
        {
            _mockRepository = new Mock<IVehicleSearchRepository>();
            _service = new VehicleSearchService(_mockRepository.Object);
            _mockLogger = new Mock<ILogger<VehicleSearchService>>();
        }

        [Fact]
        public async Task IndexVehicleAsync_ShouldCallRepositoryAndReturnDocumentId()
        {
            // Arrange
            var indexName = "vehicles";
            var vehicle = new { Make = "Toyota", Model = "Camry" };
            var expectedId = "1";

            _mockRepository
                .Setup(repo => repo.IndexVehicleAsync(indexName, vehicle))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _service.IndexVehicleAsync(indexName, vehicle);

            // Assert
            Assert.Equal(expectedId, result);
            _mockRepository.Verify(repo => repo.IndexVehicleAsync(indexName, vehicle), Times.Once);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldCallRepositoryAndReturnSearchResults()
        {
            // Arrange
            var indexName = "vehicles";
            var query = "Reliable";
            var expectedResponse = "{\"hits\": {\"total\": 1, \"hits\": [{\"_source\": {\"Description\": \"Reliable car\"}}]}}";

            _mockRepository
                .Setup(repo => repo.SearchVehiclesAsync(indexName, query))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.SearchVehiclesAsync(indexName, query);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockRepository.Verify(repo => repo.SearchVehiclesAsync(indexName, query), Times.Once);
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldCallRepository()
        {
            // Arrange
            var indexName = "vehicles";
            var vehicleId = "1";

            _mockRepository
                .Setup(repo => repo.DeleteVehicleAsync(indexName, vehicleId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteVehicleAsync(indexName, vehicleId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteVehicleAsync(indexName, vehicleId), Times.Once);
        }
    }
}