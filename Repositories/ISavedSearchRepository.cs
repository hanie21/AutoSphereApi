using AutoSphere.Api.Model;

namespace AutoSphere.Api.Repositories
{
    public interface ISavedSearchRepository
    {
        Task<SavedSearch> SaveAsync(SavedSearch savedSearch);
        Task<IEnumerable<SavedSearch>> GetByUserIdAsync(int userId);
    }
}