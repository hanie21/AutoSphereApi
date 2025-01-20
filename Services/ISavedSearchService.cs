using AutoSphere.Api.Model;

namespace AutoSphere.Api.Services
{
    public interface ISavedSearchService
    {
        Task<SavedSearch> SaveSearchAsync(SavedSearchRequest request);
        Task<IEnumerable<SavedSearch>> GetSavedSearchesAsync(int userId);
    }
}