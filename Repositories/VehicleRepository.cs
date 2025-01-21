using AutoSphere.Api.Data;
using AutoSphere.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace AutoSphere.Api.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(SearchVehicleRequest request)
        {
            var query = _context.Vehicles.AsQueryable();

           // Basic filters
            if (!string.IsNullOrEmpty(request.Make))
                query = query.Where(v => v.Make.ToLower().Contains(request.Make.ToLower()));
            if (!string.IsNullOrEmpty(request.Model))
                query = query.Where(v => v.Model.ToLower().Contains(request.Model.ToLower()));
            if (request.MinYear.HasValue)
                query = query.Where(v => v.Year >= request.MinYear.Value);
            if (request.MaxYear.HasValue)
                query = query.Where(v => v.Year <= request.MaxYear.Value);
            if (request.MinPrice.HasValue)
                query = query.Where(v => v.Price >= request.MinPrice.Value);
            if (request.MaxPrice.HasValue)
                query = query.Where(v => v.Price <= request.MaxPrice.Value);
            if (request.MinMileage.HasValue)
                query = query.Where(v => v.Mileage >= request.MinMileage.Value);
            if (request.MaxMileage.HasValue)
                query = query.Where(v => v.Mileage <= request.MaxMileage.Value);

            // Advanced filters
            if (!string.IsNullOrEmpty(request.Condition))
                query = query.Where(v => v.Condition.ToLower() == request.Condition.ToLower());
            if (request.ExcludeAccidentHistory.HasValue && request.ExcludeAccidentHistory.Value)
                query = query.Where(v => !v.HasAccidentHistory);
            if (request.MaxOwners.HasValue)
                query = query.Where(v => v.NumberOfOwners <= request.MaxOwners.Value);
            if (request.ServiceHistoryVerified.HasValue && request.ServiceHistoryVerified.Value)
                query = query.Where(v => v.ServiceHistoryVerified);
            
            if (request.Features != null && request.Features.Any())
            {
                query = query.Where(v => 
                    v.Features != null && 
                    request.Features.All(f => v.Features.Contains(f, StringComparer.OrdinalIgnoreCase))
                );
            }

            if (!string.IsNullOrEmpty(request.ExteriorColor))
                query = query.Where(v => v.ExteriorColor.ToLower() == request.ExteriorColor.ToLower());
            if (!string.IsNullOrEmpty(request.InteriorColor))
                query = query.Where(v => v.InteriorColor.ToLower() == request.InteriorColor.ToLower());
            if (!string.IsNullOrEmpty(request.EngineType))
                query = query.Where(v => v.EngineType.ToLower() == request.EngineType.ToLower());

            // Advanced filters (not repeated here for brevity)

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                query = request.SortBy.ToLower() switch
                {
                    "price" => request.SortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(v => v.Price)
                        : query.OrderBy(v => v.Price),
                    "year" => request.SortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(v => v.Year)
                        : query.OrderBy(v => v.Year),
                    "mileage" => request.SortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(v => v.Mileage)
                        : query.OrderBy(v => v.Mileage),
                    _ => query.OrderBy(v => v.Id), // Default sort
                };
            }

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            query = query.Skip(skip).Take(request.PageSize);

            return await query.ToListAsync();
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }
    }
}

