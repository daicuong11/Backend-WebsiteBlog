using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class LoveRepository : ILoveRepository
    {
        private readonly MyDbContext _context;

        public LoveRepository(MyDbContext myDbContext) 
        {
            _context = myDbContext;
        }
        public async Task<Love> Create(Love love)
        {
            var entity = await _context.Loves.AddAsync(love);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<List<Love>> GetAll()
        {
            return await _context.Loves.ToListAsync();
        }

        public async Task<Love> GetLoveID(int id)
        {
            return await _context.Loves!.SingleOrDefaultAsync(l => l.LoveID == id);
        }

        public async Task<Love> GetLoveUserIDAndArticleID(int userID, int articleID)
        {
            return await _context.Loves!.SingleOrDefaultAsync(l => l.UserTargetID == userID && l.ArticleID == articleID);
        }

        public async Task UnLove(int id)
        {
            var love = await _context.Loves.FindAsync(id);
            if(love != null)
            {
                _context.Remove(love);
                await _context.SaveChangesAsync();
            }
        }
    }
}
