namespace AutoSphere.Api.Model
{
    public class SavedSearch
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign key for user
        public string SearchName { get; set; }
        public string SearchCriteria { get; set; } // JSON string of SearchVehicleRequest
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}