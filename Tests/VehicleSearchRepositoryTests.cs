using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoSphere.Api.Repositories;
using Moq;
using OpenSearch.Net;
using Xunit;

namespace AutoSphere.Api.Tests
{
    public class VehicleSearchRepositoryTests
    {
        private readonly Mock<OpenSearchLowLevelClient> _mockClient;
        private readonly VehicleSearchRepository _repository;

        public VehicleSearchRepositoryTests()
        {
            _mockClient = new Mock<OpenSearchLowLevelClient>();
            _repository = new VehicleSearchRepository(_mockClient.Object);
        }

        [Fact]
        public async Task IndexVehicleAsync_ShouldReturnDocumentId_WhenSuccessful()
        {
            // Arrange
            var indexName = "vehicles";
            var vehicle = new { Make = "Toyota", Model = "Camry" };
            var expectedId = "1";

            // Mock response for a successful indexing
            var mockResponse = new Mock<StringResponse>();
            mockResponse.SetupGet(r => r.Success).Returns(true);
            mockResponse.SetupGet(r => r.Body).Returns("{\"_id\": \"1\"}");

            _mockClient
                .Setup(client => client.IndexAsync<StringResponse>(
                    It.Is<string>(i => i == indexName),          // Match the correct index name
                    It.Is<PostData>(d => d != null),             // Match any valid PostData
                    It.IsAny<IndexRequestParameters>(),          // Match any IndexRequestParameters
                    default(CancellationToken)                  // Explicitly pass default CancellationToken
                ))
                .ReturnsAsync(mockResponse.Object);

            // Act
            var result = await _repository.IndexVehicleAsync(indexName, vehicle);

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public async Task IndexVehicleAsync_ShouldThrowException_WhenIndexingFails()
        {
            // Arrange
            var indexName = "vehicles";
            var vehicle = new { Make = "Toyota", Model = "Camry" };

            // Mock response for a failed indexing
            var mockResponse = new Mock<StringResponse>();
            mockResponse.SetupGet(r => r.Success).Returns(false);
            mockResponse.SetupGet(r => r.Body).Returns("{\"error\": \"Indexing failed\"}");

            _mockClient
                .Setup(client => client.IndexAsync<StringResponse>(
                    It.Is<string>(i => i == indexName),          // Match the correct index name
                    It.Is<PostData>(d => d != null),             // Match any valid PostData
                    It.IsAny<IndexRequestParameters>(),          // Match any IndexRequestParameters
                    default(CancellationToken)                  // Explicitly pass default CancellationToken
                ))
                .ReturnsAsync(mockResponse.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.IndexVehicleAsync(indexName, vehicle));
            Assert.Contains("Failed to index vehicle", exception.Message);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldReturnSearchResults_WhenSuccessful()
        {
            // Arrange
            var indexName = "vehicles";
            var query = "Reliable";
            var expectedResponse = "{\"hits\": {\"total\": 1, \"hits\": [{\"_source\": {\"Description\": \"Reliable car\"}}]}}";

            // Mock response for a successful search
            var mockResponse = new Mock<StringResponse>();
            mockResponse.SetupGet(r => r.Success).Returns(true);
            mockResponse.SetupGet(r => r.Body).Returns(expectedResponse);

            _mockClient
                .Setup(client => client.SearchAsync<StringResponse>(
                    It.Is<string>(i => i == indexName),       // Match the correct index name
                    It.Is<PostData>(d => d != null),          // Match any valid PostData
                    It.IsAny<SearchRequestParameters>(),      // Match any SearchRequestParameters
                    default(CancellationToken)               // Explicitly pass the default CancellationToken
                ))
                .ReturnsAsync(mockResponse.Object);

            // Act
            var result = await _repository.SearchVehiclesAsync(indexName, query);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Reliable car", result);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldThrowException_WhenSearchFails()
        {
            // Arrange
            var indexName = "vehicles";
            var query = "Reliable";

            // Mock response for a failed search
            var mockResponse = new Mock<StringResponse>();
            mockResponse.SetupGet(r => r.Success).Returns(false);
            mockResponse.SetupGet(r => r.Body).Returns("{\"error\": \"Search failed\"}");

            _mockClient
                .Setup(client => client.SearchAsync<StringResponse>(
                    It.Is<string>(i => i == indexName),       // Match the correct index name
                    It.Is<PostData>(d => d != null),          // Match any valid PostData
                    It.IsAny<SearchRequestParameters>(),      // Match any SearchRequestParameters
                    default(CancellationToken)               // Explicitly pass the default CancellationToken
                ))
                .ReturnsAsync(mockResponse.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.SearchVehiclesAsync(indexName, query));
            Assert.Contains("Search failed", exception.Message);
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldComplete_WhenSuccessful()
        {
            // Arrange
            var indexName = "vehicles";
            var vehicleId = "1";

            var mockResponse = new Mock<StringResponse>();
            mockResponse.SetupGet(r => r.Success).Returns(true);

            _mockClient
                .Setup(client => client.DeleteAsync<StringResponse>(
                    It.Is<string>(i => i == indexName),
                    It.Is<string>(id => id == vehicleId),
                    It.IsAny<DeleteRequestParameters>(),
                    default(CancellationToken)
                ))
                .ReturnsAsync(mockResponse.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => _repository.DeleteVehicleAsync(indexName, vehicleId));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldThrowException_WhenDeletionFails()
        {
            // Arrange
            var indexName = "vehicles";
            var vehicleId = "1";

            // Mock response for a failed deletion
            var mockResponse = new Mock<StringResponse>();
            mockResponse.SetupGet(r => r.Success).Returns(false);
            mockResponse.SetupGet(r => r.Body).Returns("{\"error\": \"Deletion failed\"}");

            _mockClient
                .Setup(client => client.DeleteAsync<StringResponse>(
                    It.Is<string>(i => i == indexName),   // Match the correct index name
                    It.Is<string>(id => id == vehicleId), // Match the correct vehicle ID
                    It.IsAny<DeleteRequestParameters>(),  // Handle optional parameters
                    default(CancellationToken)           // Explicitly pass default CancellationToken
                ))
                .ReturnsAsync(mockResponse.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.DeleteVehicleAsync(indexName, vehicleId));
            Assert.Contains("Failed to delete vehicle", exception.Message);
        }
    }
}