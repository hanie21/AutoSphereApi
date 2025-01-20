using AutoSphere.Api.Model;


namespace AutoSphere.Api.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(SearchVehicleRequest request);
         Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
    }
}

