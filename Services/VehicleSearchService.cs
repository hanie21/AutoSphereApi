using AutoSphere.Api.Repositories;
using System.Text.Json;


namespace AutoSphere.Api.Services
{
    public class VehicleSearchService : IVehicleSearchService
    {
        private readonly IVehicleSearchRepository _repository;

        public VehicleSearchService(IVehicleSearchRepository repository)
        {
            _repository = repository;
        }

        public async Task<string>  IndexVehicleAsync(string indexName, object vehicle)
        {
            return await _repository.IndexVehicleAsync(indexName, vehicle);
        }

        public async Task<string> SearchVehiclesAsync(string indexName, string query)
        {
            return await _repository.SearchVehiclesAsync(indexName, query);
        }

        public async Task DeleteVehicleAsync(string indexName, string vehicleId)
        {
            await _repository.DeleteVehicleAsync(indexName, vehicleId);
        }

        public async Task<string> FuzzyMatchingSearchVehicleAsync(string indexName, string query)
        {
            return await _repository.SearchVehiclesWithFuzzyMatchingAsync(indexName, query);
        }
    }
}