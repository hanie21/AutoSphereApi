using System;
using AutoSphere.Api.Model;
using System.Threading.Tasks;

namespace AutoSphere.Api.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(SearchVehicleRequest request);
        Task<Vehicle> AddVehicleAsync(AddVehicleRequest request);
    }
}

