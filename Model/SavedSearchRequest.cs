
namespace AutoSphere.Api.Model
{
    public class SavedSearchRequest
{
    public int UserId { get; set; } // User making the request
    public string SearchName { get; set; }
    public SearchVehicleRequest Criteria { get; set; } // The search criteria being saved
}
}