using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys.Services
{
    public class ContentRepository : IContentRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ContentRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<Content> CreateContent(ContentModal content)
        {
            var entityEntry = await _context.Contents.AddAsync(_mapper.Map<Content>(content));
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task UpdateContent(ContentModal content)
        {
            _context.Contents.Update(_mapper.Map<Content>(content));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContent(Content content)
        {
            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();
        }
    }
}
