using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class LikeRepository : ILikeRepository
    {
        private readonly MyDbContext _context;
        public LikeRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Like> GetLikeByUserIdAndArticleId(int userId, int articleId)
        {
            return await _context.Likes.FirstOrDefaultAsync(like => like.UserID == userId && like.ArticleID == articleId);
        }


        public async Task<Like> Like(Like item)
        {
            var entityEntry = await _context.Likes.AddAsync(item);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task UnLike(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
    }
}
