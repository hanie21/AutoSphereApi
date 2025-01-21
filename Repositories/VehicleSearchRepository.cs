using OpenSearch.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AutoSphere.Api.Repositories
{
    public class VehicleSearchRepository : IVehicleSearchRepository
    {
        private readonly OpenSearchLowLevelClient _client;
        private readonly ILogger<VehicleSearchRepository> _logger;

        public VehicleSearchRepository(OpenSearchLowLevelClient client, ILogger<VehicleSearchRepository> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<string> IndexVehicleAsync(string indexName, object vehicle)
        {
            // Perform the indexing operation
            var response = await _client.IndexAsync<StringResponse>(indexName, PostData.Serializable(vehicle));

            // Check if the indexing operation was successful
            if (!response.Success)
            {
                // Log the error using Serilog for debugging
                _logger.LogError("Failed to index vehicle. Response: {ResponseBody}", response.Body);

                // The error handling middleware will take care of logging and response management
                throw new Exception($"Failed to index vehicle: {response.Body}");
            }

            // Parse the response to extract the document ID
            var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(response.Body);
            if (responseObject != null && responseObject.TryGetValue("_id", out var id))
            {
                return id.ToString();
            }

            // Log the error if document ID could not be retrieved
            _logger.LogError("Indexing succeeded, but document ID could not be retrieved from the response.");
            
            // Error handling middleware will log and manage the exception
            throw new Exception("Indexing succeeded, but document ID could not be retrieved.");
        }

        public async Task<string> SearchVehiclesAsync(string indexName, string query)
        {
            var searchQuery = new
            {
                query = new
                {
                    match = new { Description = query }
                }
            };

            var response = await _client.SearchAsync<StringResponse>(indexName, PostData.Serializable(searchQuery));
            if (!response.Success)
            {
                throw new Exception($"Search failed: {response.Body}");
            }

            return response.Body;
        }

        public async Task DeleteVehicleAsync(string indexName, string vehicleId)
        {
            var response = await _client.DeleteAsync<StringResponse>(indexName, vehicleId);
            if (!response.Success)
            {
                throw new Exception($"Failed to delete vehicle: {response.Body}");
            }
        }
    
        public async Task<string> SearchVehiclesWithFuzzyMatchingAsync(string indexName, string query)
        {
            var fuzzyQuery = new
            {
                query = new
                {
                    fuzzy = new
                    {
                        make = new
                        {
                            value = query,
                            fuzziness = "AUTO"  // Automatic fuzziness based on the length of the term
                        }
                    }
                }
            };

            var response = await _client.SearchAsync<StringResponse>(indexName, PostData.Serializable(fuzzyQuery));

            if (!response.Success)
            {
                throw new Exception($"Search failed: {response.Body}");
            }

            return response.Body;
        }
    
    }
}