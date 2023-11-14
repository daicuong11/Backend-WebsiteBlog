using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyDbContext _context;
        public CommentRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            var entityEntry = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentByParentCommentID(int id)
        {
            return await _context.Comments.Where(c => c.ParentCommentID == id).ToListAsync();
        }

        public async Task<List<Comment>> GetCommentByUserIdAndArticleId(int userId, int articleId)
        {
            return await _context.Comments.Where(c => c.UserID == userId && c.ArticleID == articleId).ToListAsync();
        }

        public async Task UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
