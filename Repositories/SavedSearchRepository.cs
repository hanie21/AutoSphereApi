using AutoSphere.Api.Model;
using AutoSphere.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoSphere.Api.Repositories
{
    public class SavedSearchRepository : ISavedSearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedSearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SavedSearch> SaveAsync(SavedSearch savedSearch)
        {
            _context.SavedSearches.Add(savedSearch);
            await _context.SaveChangesAsync();
            return savedSearch;
        }

        public async Task<IEnumerable<SavedSearch>> GetByUserIdAsync(int userId)
        {
            return await _context.SavedSearches
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }
    }
}