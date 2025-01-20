using AutoSphere.Api.Model;
using AutoSphere.Api.Repositories;
using AutoSphere.Api.Services;
using Moq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AutoSphere.Api.Tests
{
    public class SavedSearchServiceTests
    {
        private readonly Mock<ISavedSearchRepository> _mockSavedSearchRepository;
        private readonly SavedSearchService _savedSearchService;

        public SavedSearchServiceTests()
        {
            _mockSavedSearchRepository = new Mock<ISavedSearchRepository>();
            _savedSearchService = new SavedSearchService(_mockSavedSearchRepository.Object);
        }

        [Fact]
        public async Task SaveSearchAsync_ShouldSaveSearchSuccessfully()
        {
            // Arrange
            var request = new SavedSearchRequest
            {
                UserId = 1,
                SearchName = "Test Search",
                Criteria = new SearchVehicleRequest { Make = "Toyota" } 
            };

            var savedSearch = new SavedSearch
            {
                UserId = request.UserId,
                SearchName = request.SearchName,
                SearchCriteria = JsonSerializer.Serialize(request.Criteria)
            };

            // Setup the mock to return the saved search
            _mockSavedSearchRepository
                .Setup(repo => repo.SaveAsync(It.IsAny<SavedSearch>()))
                .ReturnsAsync(savedSearch);

            // Act
            var result = await _savedSearchService.SaveSearchAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(savedSearch.UserId, result.UserId);
            Assert.Equal(savedSearch.SearchName, result.SearchName);
            Assert.Equal(savedSearch.SearchCriteria, result.SearchCriteria);
        }

        [Fact]
        public async Task GetSavedSearchesAsync_ShouldReturnSavedSearches()
        {
            // Arrange
            var userId = 1;
            var savedSearches = new List<SavedSearch>
            {
                new SavedSearch
                {
                    UserId = userId,
                    SearchName = "Test Search 1",
                    SearchCriteria = "{\"make\": \"Toyota\"}"
                },
                new SavedSearch
                {
                    UserId = userId,
                    SearchName = "Test Search 2",
                    SearchCriteria = "{\"make\": \"Honda\"}"
                }
            };

            // Setup the mock to return saved searches
            _mockSavedSearchRepository
                .Setup(repo => repo.GetByUserIdAsync(userId))
                .ReturnsAsync(savedSearches);

            // Act
            var result = await _savedSearchService.GetSavedSearchesAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, ss => ss.SearchName == "Test Search 1");
            Assert.Contains(result, ss => ss.SearchName == "Test Search 2");
        }
    }
}