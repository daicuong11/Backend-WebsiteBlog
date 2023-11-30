using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class SaveArticleRepository : ISaveArticleRepository
    {
        private readonly MyDbContext _context;

        public SaveArticleRepository(MyDbContext context) { _context = context; }

        public async Task<SavedArticle> Create(SavedArticle savedArticle)
        {
            var entitesEntry = await _context.SavedArticles.AddAsync(savedArticle);
            await _context.SaveChangesAsync();
            return entitesEntry.Entity;
        }

        public async Task Delete(SavedArticle savedArticle)
        {
            _context.Remove(savedArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SavedArticle>> GetAllByUserID(int id)
        {
            return await _context.SavedArticles
                .Where(n => n.UserTargetID == id)
                .ToListAsync();
        }

        public async Task<SavedArticle> GetSavedByUserIDAndArticleID(int userID, int articleID)
        {
            return await _context.SavedArticles.SingleOrDefaultAsync(s => s.ArticleID == articleID && s.UserTargetID == userID);
        }

        public async Task<List<SavedArticle>> GetSavedOfUserTargetID(int id, int pageNumber, int pageSize)
        {
            return await _context.SavedArticles
                .Where(n => n.UserTargetID == id)
                .Include(n => n.Article)
                .OrderByDescending(n => n.SavedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
