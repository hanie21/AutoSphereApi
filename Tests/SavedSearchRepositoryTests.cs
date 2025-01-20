using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoSphere.Api.Data;
using AutoSphere.Api.Model;
using AutoSphere.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoSphere.Api.Tests.Repositories
{
    public class SavedSearchRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly SavedSearchRepository _repository;

        public SavedSearchRepositoryTests()
        {
            // Initialize the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SavedSearchTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new SavedSearchRepository(_context);

            // Seed data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var savedSearches = new List<SavedSearch>
            {
                new SavedSearch { Id = 1, UserId = 101, SearchName = "SUV Search", SearchCriteria = "{ \"make\": \"Toyota\" }" },
                new SavedSearch { Id = 2, UserId = 102, SearchName = "Luxury Cars", SearchCriteria = "{ \"make\": \"BMW\" }" },
                new SavedSearch { Id = 3, UserId = 101, SearchName = "Budget Cars", SearchCriteria = "{ \"price\": \"<20000\" }" }
            };

            _context.SavedSearches.AddRange(savedSearches);
            _context.SaveChanges();
        }

        [Fact]
        public async Task SaveAsync_ShouldSaveSavedSearchToDatabase()
        {
            // Arrange
            var newSavedSearch = new SavedSearch
            {
                UserId = 103,
                SearchName = "Electric Cars",
                SearchCriteria = "{ \"make\": \"Tesla\" }"
            };

            // Act
            var result = await _repository.SaveAsync(newSavedSearch);
            var savedSearchInDb = await _context.SavedSearches.FirstOrDefaultAsync(s => s.Id == result.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newSavedSearch.UserId, result.UserId);
            Assert.Equal(newSavedSearch.SearchName, result.SearchName);
            Assert.NotNull(savedSearchInDb);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnSavedSearchesForUser()
        {
            // Arrange
            var userId = 101;

            // Act
            var result = await _repository.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, search => Assert.Equal(userId, search.UserId));
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnEmptyList_WhenNoSearchesExistForUser()
        {
            // Arrange
            var userId = 999; // User with no saved searches

            // Act
            var result = await _repository.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}