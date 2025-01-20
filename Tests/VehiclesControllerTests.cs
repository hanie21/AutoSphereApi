using AutoSphere.Api.Controllers;
using AutoSphere.Api.Model;
using AutoSphere.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace AutoSphere.Api.Tests.Controllers
{
    public class VehiclesControllerTests
    {
        private readonly Mock<IMemoryCache> _cache; 
        private readonly Mock<ILogger<VehiclesController>> _mockLogger;
        private readonly Mock<IVehicleService> _mockVehicleService;
        private readonly Mock<ISavedSearchService> _mockSavedSearchService;
        private readonly Mock<IVehicleSearchService> _mockVehicleSearchService;
        private readonly VehiclesController _controller;

        public VehiclesControllerTests()
        {
            _cache = new Mock<IMemoryCache>();
             _mockLogger = new Mock<ILogger<VehiclesController>>();
            _mockVehicleService = new Mock<IVehicleService>();
            _mockSavedSearchService = new Mock<ISavedSearchService>();
            _mockVehicleSearchService = new Mock<IVehicleSearchService>();
            _controller = new VehiclesController(
                _mockVehicleService.Object,
                _mockSavedSearchService.Object,
                _mockVehicleSearchService.Object,
                _mockLogger.Object,
                _cache.Object
            );
        }

        [Fact]
        public void Test_With_MockedLogger()
        {
            // Act
            var result = _controller.TestLog();

            // Assert
            Assert.IsType<OkObjectResult>(result);

            // Verify that the logger was called with specific log messages
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("This is an informational message.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );

            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("This is an error message with additional context: SampleContext")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Search_ShouldReturnBadRequest_WhenPageNumberOrPageSizeIsInvalid()
        {
            // Arrange
            var request = new SearchVehicleRequest { PageNumber = 0, PageSize = 0 };

            // Act
            var result = await _controller.Search(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("PageNumber and PageSize must be greater than zero.", badRequestResult.Value);
        }

        [Fact]
        public async Task Search_ShouldReturnOk_WithSearchResults()
        {
            // Arrange
            var request = new SearchVehicleRequest { PageNumber = 1, PageSize = 10 };
            var searchResults = new List<Vehicle> { new Vehicle { Id = 1, Make = "Toyota", Model = "Camry", Description = "A luxurious and well-maintained 2022 BMW X5 with low mileage and premium features, including a sunroof and navigation system."} };
            _mockVehicleService.Setup(s => s.SearchVehiclesAsync(request)).ReturnsAsync(searchResults);

            // Act
            var result = await _controller.Search(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(searchResults, okResult.Value);
        }

        [Fact]
        public async Task SearchVehicles_ShouldReturnOk_WithSearchResults()
        {
            // Arrange
            var query = "Reliable";
            var searchResults = "{\"hits\": [{\"_source\": {\"Description\": \"Reliable car\"}}]}";
            _mockVehicleSearchService.Setup(s => s.SearchVehiclesAsync("vehicles", query)).ReturnsAsync(searchResults);

            // Act
            var result = await _controller.SearchVehicles(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(searchResults, okResult.Value);
        }

        [Fact]
        public async Task IndexTo_ShouldReturnOk_WhenIndexingSucceeds()
        {
            // Arrange
            var request = new AddVehicleRequest { Make = "Toyota", Model = "Camry" };
            var documentId = "1";
            _mockVehicleSearchService.Setup(s => s.IndexVehicleAsync("vehicles", request)).ReturnsAsync(documentId);

            // Act
            var result = await _controller.IndexTo(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(documentId, okResult.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WhenVehicleIsAdded()
        {
            // Arrange
            var request = new AddVehicleRequest { Make = "Toyota", Model = "Camry" };
            var addedVehicle = new Vehicle { Id = 1, Make = "Toyota", Model = "Camry",Description = "A luxurious and well-maintained 2022 BMW X5 with low mileage and premium features, including a sunroof and navigation system." };
            _mockVehicleService.Setup(s => s.AddVehicleAsync(request)).ReturnsAsync(addedVehicle);

            // Act
            var result = await _controller.Add(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(addedVehicle, createdResult.Value);
            Assert.Equal(nameof(_controller.Search), createdResult.ActionName);
        }

        [Fact]
        public async Task SaveSearch_ShouldReturnOk_WithSavedSearch()
        {
            // Arrange
            var searchCriteria = new SearchVehicleRequest { Make = "Toyota" }; // Use the actual object type
            var request = new SavedSearchRequest
            {
                SearchName = "Test Search",
                Criteria = searchCriteria // Assign the correct type here
            };

            var savedSearch = new SavedSearch
            {
                Id = 1,
                UserId = 101,
                SearchName = "Test Search",
                SearchCriteria = JsonSerializer.Serialize(searchCriteria) // Serialized object for the saved search
            };

            _mockSavedSearchService
                .Setup(s => s.SaveSearchAsync(request))
                .ReturnsAsync(savedSearch);

            // Act
            var result = await _controller.SaveSearch(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(savedSearch, okResult.Value);
        }

        [Fact]
        public async Task GetSavedSearches_ShouldReturnOk_WithSavedSearches()
        {
            // Arrange
            var userId = 101;
            var savedSearches = new List<SavedSearch>
            {
                new SavedSearch { Id = 1, UserId = 101, SearchName = "Test Search 1" },
                new SavedSearch { Id = 2, UserId = 101, SearchName = "Test Search 2" }
            };
            _mockSavedSearchService.Setup(s => s.GetSavedSearchesAsync(userId)).ReturnsAsync(savedSearches);

            // Act
            var result = await _controller.GetSavedSearches(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(savedSearches, okResult.Value);
        }
    }
}