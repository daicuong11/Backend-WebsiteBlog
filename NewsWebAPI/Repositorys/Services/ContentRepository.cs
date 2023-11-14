using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class ContentRepository : IContentRepository
    {
        private readonly MyDbContext _context;

        public ContentRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Content>> GetAllContents()
        {
            return await _context.Contents.ToListAsync();
        }
        public async Task<List<Content>> GetAllContentByArticleID(int articleId)
        {
            return await _context.Contents
                .Where(c => c.ArticleID == articleId)
                .OrderBy(c => c.ContentID)
                .ToListAsync();
        }

        public async Task<Content> GetContentById(int contentId)
        {
            return await _context.Contents.FindAsync(contentId);
        }

        public async Task<Content> CreateContent(Content content)
        {
            var entityEntry = await _context.Contents.AddAsync(content);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task UpdateContent(Content content)
        {
            _context.Contents.Update(content);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContent(Content content)
        {
            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();
        }
    }
}
