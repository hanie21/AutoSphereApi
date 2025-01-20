using AutoSphere.Api.Model;
using System.Text.Json;
using AutoSphere.Api.Repositories;

namespace AutoSphere.Api.Services
{
    public class SavedSearchService: ISavedSearchService
    {
        private readonly ISavedSearchRepository _repository;

        public SavedSearchService(ISavedSearchRepository repository)
        {
            _repository = repository;
        }
        public async Task<SavedSearch> SaveSearchAsync(SavedSearchRequest request)
        {
            var savedSearch = new SavedSearch
            {
                UserId = request.UserId,
                SearchName = request.SearchName,
                SearchCriteria = JsonSerializer.Serialize(request.Criteria)
            };

            return await _repository.SaveAsync(savedSearch);
        }

        public async Task<IEnumerable<SavedSearch>> GetSavedSearchesAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}