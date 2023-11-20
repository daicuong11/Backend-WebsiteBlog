using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ArticleRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(a => a.ArticleID == id);
        }

        public async Task<List<Article>> GetAllArticles()
        {
            return await _context.Articles
                .Include(a => a.User)
                .ToListAsync();
        }

        //phân trang
        public async Task<List<Article>> GetPagedArticles(int pageNumber, int pageSize)
        {
            var articles = await _context.Articles
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return articles;
        }

        public async Task<List<Article>> GetPagedArticlesByCategoryID(int id, int pageNumber, int pageSize)
        {
            return await _context.Articles
                .Where(a => a.CategoryID == id)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Article> CreateArticle(ArticleModal article)
        {
            var newArticle = _mapper.Map<Article>(article);
            var entityEntry = await _context.Articles.AddAsync(newArticle);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task UpdateArticle(ArticleModal article)
        {
            var newArticle = _mapper.Map<Article>(article);
            _context.Articles!.Update(newArticle);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteArticle(ArticleModal article)
        {
            var newArticle = _mapper.Map<Article>(article);
            _context.Articles.Remove(newArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Article>> GetAllArticlesByCategoryID(int id)
        {
            return await _context.Articles.Where(a =>  a.CategoryID == id).ToListAsync();
        }

    }
}
