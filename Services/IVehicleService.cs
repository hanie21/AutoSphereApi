using AutoSphere.Api.Model;

namespace AutoSphere.Api.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(SearchVehicleRequest request);
        Task<Vehicle> AddVehicleAsync(AddVehicleRequest request);
    }
}

