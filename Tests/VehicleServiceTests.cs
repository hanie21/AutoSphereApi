using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoSphere.Api.Model;
using AutoSphere.Api.Repositories;
using AutoSphere.Api.Services;
using Moq;
using Xunit;

namespace AutoSphere.Api.Tests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _mockVehicleRepository;
        private readonly Mock<IVehicleSearchService> _mockVehicleSearchService;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            // Mocking dependencies
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _mockVehicleSearchService = new Mock<IVehicleSearchService>();

            // Initializing VehicleService with mocked dependencies
            _vehicleService = new VehicleService(_mockVehicleRepository.Object, _mockVehicleSearchService.Object);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldReturnVehicles()
        {
            // Arrange: Set up mock behavior for SearchVehiclesAsync method in the repository
            var searchRequest = new SearchVehicleRequest { Make = "Toyota" };
            var mockVehicles = new List<Vehicle>
                {
                    new Vehicle 
                    { 
                        Id = 1, 
                        Make = "Toyota", 
                        Model = "Camry", 
                        Description = "A reliable Toyota Camry" // Set the required property
                    },
                    new Vehicle 
                    { 
                        Id = 2, 
                        Make = "Toyota", 
                        Model = "Corolla", 
                        Description = "A dependable Toyota Corolla" // Set the required property
                    }
                };

            _mockVehicleRepository
                .Setup(repo => repo.SearchVehiclesAsync(It.IsAny<SearchVehicleRequest>()))
                .ReturnsAsync(mockVehicles);

            // Act: Call the method
            var result = await _vehicleService.SearchVehiclesAsync(searchRequest);

            // Assert: Check if the result matches the expected data
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, v => v.Make == "Toyota");
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldAddVehicleAndIndexIt()
        {
            // Arrange: Set up mock behavior for AddVehicleAsync and IndexVehicleAsync methods
            var addVehicleRequest = new AddVehicleRequest
            {
                Make = "Tesla",
                Model = "Model S",
                Year = 2022,
                Price = 80000,
                Mileage = 5000,
                BodyType = "Sedan",
                Transmission = "Automatic",
                FuelType = "Electric",
                Location = "San Francisco",
                Condition = "New",
                HasAccidentHistory = false,
                NumberOfOwners = 0,
                ServiceHistoryVerified = true,
                Features = new List<string> { "Autopilot", "Full Self-Driving" },
                ExteriorColor = "Red",
                InteriorColor = "White",
                EngineType = "Electric",
                Description = "Brand-new Tesla Model S"
            };

            var mockVehicle = new Vehicle
            {
                Id = 1,
                Make = "Tesla",
                Model = "Model S",
                Year = 2022,
                Price = 80000,
                Mileage = 5000,
                BodyType = "Sedan",
                Transmission = "Automatic",
                FuelType = "Electric",
                Location = "San Francisco",
                Condition = "New",
                HasAccidentHistory = false,
                NumberOfOwners = 0,
                ServiceHistoryVerified = true,
                Features = new List<string> { "Autopilot", "Full Self-Driving" },
                ExteriorColor = "Red",
                InteriorColor = "White",
                EngineType = "Electric",
                Description = "Brand-new Tesla Model S"
            };

            _mockVehicleRepository
                .Setup(repo => repo.AddVehicleAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync(mockVehicle);

            
            _mockVehicleSearchService
                .Setup(service => service.IndexVehicleAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult(string.Empty)); 

            // Act: Call the method
            var result = await _vehicleService.AddVehicleAsync(addVehicleRequest);

            // Assert: Verify that the vehicle was added and indexed
            Assert.NotNull(result);
            Assert.Equal(mockVehicle.Id, result.Id);
            _mockVehicleSearchService.Verify(service => service.IndexVehicleAsync("vehicles", result), Times.Once);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldHandleSearchFailure()
        {
            // Arrange: Set up mock behavior for AddVehicleAsync and simulate an exception during indexing
            var addVehicleRequest = new AddVehicleRequest
            {
                Make = "Tesla",
                Model = "Model S",
                Year = 2022,
                Price = 80000,
                Mileage = 5000,
                BodyType = "Sedan",
                Transmission = "Automatic",
                FuelType = "Electric",
                Location = "San Francisco",
                Condition = "New",
                HasAccidentHistory = false,
                NumberOfOwners = 0,
                ServiceHistoryVerified = true,
                Features = new List<string> { "Autopilot", "Full Self-Driving" },
                ExteriorColor = "Red",
                InteriorColor = "White",
                EngineType = "Electric",
                Description = "Brand-new Tesla Model S"
            };

            var mockVehicle = new Vehicle
            {
                Id = 1,
                Make = "Tesla",
                Model = "Model S",
                Year = 2022,
                Price = 80000,
                Mileage = 5000,
                BodyType = "Sedan",
                Transmission = "Automatic",
                FuelType = "Electric",
                Location = "San Francisco",
                Condition = "New",
                HasAccidentHistory = false,
                NumberOfOwners = 0,
                ServiceHistoryVerified = true,
                Features = new List<string> { "Autopilot", "Full Self-Driving" },
                ExteriorColor = "Red",
                InteriorColor = "White",
                EngineType = "Electric",
                Description = "Brand-new Tesla Model S"
            };

            _mockVehicleRepository
                .Setup(repo => repo.AddVehicleAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync(mockVehicle);

            _mockVehicleSearchService
                .Setup(service => service.IndexVehicleAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Throws(new Exception("Failed to index vehicle"));

            // Act & Assert: Call AddVehicleAsync and check that the exception during indexing is handled
            var exception = await Assert.ThrowsAsync<Exception>(() => _vehicleService.AddVehicleAsync(addVehicleRequest));
            Assert.Equal("Failed to index vehicle", exception.Message);
        }
    }
}