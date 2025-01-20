namespace AutoSphere.Api.Model
{
    public class Favorite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VehicleId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
}