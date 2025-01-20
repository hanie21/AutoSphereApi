namespace AutoSphere.Api.Model
{
    public class AddVehicleRequest
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string BodyType { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; } // e.g., New, Used, Certified Pre-Owned
        public bool HasAccidentHistory { get; set; } // Indicates if there’s any accident history
        public int NumberOfOwners { get; set; } // Number of previous owners
        public bool ServiceHistoryVerified { get; set; } // Indicates if the service history is verified
        public List<string> Features { get; set; } = new(); // Advanced features (e.g., Sunroof, Navigation System)
        public string ExteriorColor { get; set; } // Exterior color of the vehicle
        public string InteriorColor { get; set; } // Interior color of the vehicle
        public string EngineType { get; set; } // Engine type (e.g., V6, Turbocharged)
        public string Description { get; set; } // Detailed description of the vehicle
    }
}