using OpenSearch.Net;
using System.Text.Json;

namespace AutoSphere.Api.Repositories
{
    public class VehicleSearchRepository : IVehicleSearchRepository
    {
        private readonly OpenSearchLowLevelClient _client;

        public VehicleSearchRepository(OpenSearchLowLevelClient client)
        {
            _client = client;
        }

        public async Task<string> IndexVehicleAsync(string indexName, object vehicle)
        {
            var response = await _client.IndexAsync<StringResponse>(indexName, PostData.Serializable(vehicle));
            
            if (!response.Success)
            {
                // Log the detailed error for debugging
                Console.WriteLine($"Failed to index vehicle: {response.Body}");
                throw new Exception($"Failed to index vehicle: {response.Body}");
            }

            // Parse and return the ID of the indexed document
            var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(response.Body);
            if (responseObject != null && responseObject.TryGetValue("_id", out var id))
            {
                return id.ToString();
            }

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
    }
}