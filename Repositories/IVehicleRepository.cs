using System.Threading.Tasks;
using AutoSphere.Api.Model;
using Microsoft.AspNetCore.Mvc;


namespace AutoSphere.Api.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(SearchVehicleRequest request);
         Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
    }
}

