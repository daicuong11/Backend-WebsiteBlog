using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetNotifiOfUserTargetID(int id);
        Task<List<Notification>> GetNotifiOfUserCreateID(int id);
        Task<Notification> GetNotifiByID(int id);
        Task<Notification> Create(Notification notify);
        Task IsRead(int id);
    }
}
