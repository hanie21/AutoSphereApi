namespace AutoSphere.Api.Model
{
    public class SearchHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SearchCriteria { get; set; }
    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
}
}