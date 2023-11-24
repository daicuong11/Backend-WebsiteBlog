using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MyDbContext _context;

        public NotificationRepository(MyDbContext myDbContext) { _context = myDbContext; }
        public async Task<Notification> Create(Notification notify)
        {
            var entitesEntry = await _context.Notifications.AddAsync(notify);
            await _context.SaveChangesAsync();
            return entitesEntry.Entity;
        }

        public async Task<Notification> GetNotifiByID(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<List<Notification>> GetNotifiOfUserTargetID(int id)
        {
            return await _context.Notifications
                .Where(n => n.UserTargetID == id)
                .Include(n => n.UserCreate)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        }

        public async Task IsRead(int id)
        {
            var notify = await _context.Notifications.FindAsync(id);
            notify.IsRead = true;
            _context.Update(notify);
            await _context.SaveChangesAsync();
        }
    }
}
