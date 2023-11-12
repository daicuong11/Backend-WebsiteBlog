using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MyDbContext _context;
        public ArticleRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            var entityEntry = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<Like> AddLike(Like like)
        {
            var entityEntry = await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<List<Article>> GetAllArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<List<Like>> GetLikesForArticle(int articleId)
        {
            return await _context.Likes.Where(l => l.ArticleId == articleId).ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsForArticle(int articleId)
        {
            return await _context.Comments
                .Include(c => c.Replies)
                .Where(c => c.ArticleId == articleId && c.ParentCommentId == null)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<Article> CreateArticle(Article article)
        {
            var entityEntry = await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task UpdateArticle(Article article)
        {
            _context.Articles!.Update(article);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteArticle(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}
