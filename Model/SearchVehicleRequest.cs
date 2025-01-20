using System;
namespace AutoSphere.Api.Model
{
    public class SearchVehicleRequest
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinMileage { get; set; }
        public int? MaxMileage { get; set; }
        public string? BodyType { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }
        public string? Location { get; set; }
        public string? Condition { get; set; } // New, Certified, Used
        public bool? ExcludeAccidentHistory { get; set; } // Include/exclude accidents
        public int? MaxOwners { get; set; } // Maximum number of owners
        public bool? ServiceHistoryVerified { get; set; } // Verified maintenance records
        public List<string?>? Features { get; set; } // e.g., Sunroof, Navigation, Heated Seats
        public string? ExteriorColor { get; set; }
        public string? InteriorColor { get; set; }
        public string? EngineType { get; set; } // e.g., V6, V8, Turbocharged

        // New fields for sorting and pagination
        public string SortBy { get; set; } // e.g., Price, Year, Mileage
        public string SortDirection { get; set; } = "asc"; // asc or desc
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 10; // Default to 10 results per page
    }
}

