namespace AutoSphere.Api.Model
{
    public class Vehicle
    {
        public int Id { get; set; }
        public required string Make { get; set; } = string.Empty;
        public required string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string? BodyType { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }
        public string? Location { get; set; }
        public string? Condition { get; set; }
        public bool HasAccidentHistory { get; set; }
        public int NumberOfOwners { get; set; }
        public bool ServiceHistoryVerified { get; set; }
        public List<string> Features { get; set; } = new();
        public string ExteriorColor { get; set; }
        public string? InteriorColor { get; set; }
        public string? EngineType { get; set; }
        public required string Description { get; set; } // Detailed description of the vehicle
    }
}

