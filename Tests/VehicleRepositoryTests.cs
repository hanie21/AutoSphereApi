using AutoSphere.Api.Data;
using AutoSphere.Api.Model;
using AutoSphere.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoSphere.Api.Tests
{
    public class VehicleRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly VehicleRepository _repository;

        public VehicleRepositoryTests()
        {
            // Initialize the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new VehicleRepository(_context);

            // Seed data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 1,
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2020,
                    Price = 20000,
                    Mileage = 30000,
                    BodyType = "Sedan",
                    FuelType = "Gasoline",
                    Transmission = "Automatic",
                    Location = "Los Angeles",
                    Condition = "Used",
                    HasAccidentHistory = false,
                    NumberOfOwners = 1,
                    ServiceHistoryVerified = true,
                    Features = new List<string> { "Sunroof", "Navigation System" },
                    ExteriorColor = "Black",
                    InteriorColor = "Beige",
                    EngineType = "V6",
                    Description = "A reliable used Toyota Camry in great condition."
                },
                new Vehicle
                {
                    Id = 2,
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2021,
                    Price = 22000,
                    Mileage = 15000,
                    BodyType = "Sedan",
                    FuelType = "Gasoline",
                    Transmission = "Automatic",
                    Location = "New York",
                    Condition = "New",
                    HasAccidentHistory = false,
                    NumberOfOwners = 0,
                    ServiceHistoryVerified = true,
                    Features = new List<string> { "Bluetooth", "Backup Camera" },
                    ExteriorColor = "White",
                    InteriorColor = "Black",
                    EngineType = "Inline-4",
                    Description = "A new Honda Civic with advanced safety features."
                },
                new Vehicle
                {
                    Id = 3,
                    Make = "BMW",
                    Model = "X5",
                    Year = 2023,
                    Price = 75000,
                    Mileage = 10000,
                    BodyType = "SUV",
                    FuelType = "Diesel",
                    Transmission = "Automatic",
                    Location = "Chicago",
                    Condition = "Used",
                    HasAccidentHistory = true,
                    NumberOfOwners = 1,
                    ServiceHistoryVerified = false,
                    Features = new List<string> { "Heated Seats", "Panoramic Roof" },
                    ExteriorColor = "Blue",
                    InteriorColor = "Tan",
                    EngineType = "Turbocharged V6",
                    Description = "A luxury BMW X5 with minor accident history."
                }
            };

            _context.Vehicles.AddRange(vehicles);
            _context.SaveChanges();
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldReturnFilteredResults()
        {
            // Arrange
            var searchRequest = new SearchVehicleRequest
            {
                Make = "Toyota",
                MinYear = 2019,
                MaxPrice = 25000
            };

            // Act
            var result = await _repository.SearchVehiclesAsync(searchRequest);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(result, v => v.Make == "Toyota" && v.Year == 2020);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var searchRequest = new SearchVehicleRequest
            {
                PageNumber = 1,
                PageSize = 2
            };

            // Act
            var result = await _repository.SearchVehiclesAsync(searchRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldAddVehicleToDatabase()
        {
            // Arrange
            var newVehicle = new Vehicle
            {
                Make = "Tesla",
                Model = "Model S",
                Year = 2022,
                Price = 80000,
                Mileage = 5000,
                BodyType = "Sedan",
                FuelType = "Electric",
                Transmission = "Automatic",
                Location = "San Francisco",
                Condition = "New",
                HasAccidentHistory = false,
                NumberOfOwners = 0,
                ServiceHistoryVerified = true,
                Features = new List<string> { "Autopilot", "Full Self-Driving Capability" },
                ExteriorColor = "Red",
                InteriorColor = "White",
                EngineType = "Electric",
                Description = "A brand-new Tesla Model S with cutting-edge features."
            };

            // Act
            var result = await _repository.AddVehicleAsync(newVehicle);
            var vehicleInDb = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == result.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(vehicleInDb);
            Assert.Equal("Tesla", vehicleInDb.Make);
        }
    }
}