using System;
using AutoSphere.Api.Model;
using AutoSphere.Api.Repositories;

namespace AutoSphere.Api.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
         private readonly IVehicleSearchService _vehicleSearchService;

        public VehicleService(IVehicleRepository vehicleRepository, IVehicleSearchService vehicleSearchService) 
        {
            _vehicleRepository = vehicleRepository;
            _vehicleSearchService = vehicleSearchService;
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(SearchVehicleRequest request)
        {
            return await _vehicleRepository.SearchVehiclesAsync(request);
        }

        public async Task<Vehicle> AddVehicleAsync(AddVehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                Price = request.Price,
                Mileage = request.Mileage,
                BodyType = request.BodyType,
                Transmission = request.Transmission,
                FuelType = request.FuelType,
                Location = request.Location,
                Condition = request.Condition,
                HasAccidentHistory = request.HasAccidentHistory,
                NumberOfOwners = request.NumberOfOwners,
                ServiceHistoryVerified = request.ServiceHistoryVerified,
                Features = request.Features,
                ExteriorColor = request.ExteriorColor,
                InteriorColor = request.InteriorColor,
                EngineType = request.EngineType,
                Description =   request.Description

            };

            var savedVehicle = await _vehicleRepository.AddVehicleAsync(vehicle);
            try
            {
                await _vehicleSearchService.IndexVehicleAsync("vehicles", savedVehicle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to index vehicle in OpenSearch: {ex.Message}");
            }
            return savedVehicle;
        }
    }
}

