using AutoSphere.Api.Model;

namespace AutoSphere.Api.Services
{
    public interface IVehicleSearchService
    {
        Task<string> IndexVehicleAsync(string indexName, object vehicle);
        Task<string> SearchVehiclesAsync(string indexName, string query);
        Task DeleteVehicleAsync(string indexName, string vehicleId);
    }
}