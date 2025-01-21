namespace AutoSphere.Api.Repositories
{
    public interface IVehicleSearchRepository
    {
         Task<string> IndexVehicleAsync(string indexName, object vehicle);
        Task<string> SearchVehiclesAsync(string indexName, string query);
        Task DeleteVehicleAsync(string indexName, string vehicleId);
        Task<string> SearchVehiclesWithFuzzyMatchingAsync(string indexName, string query);
    }
}