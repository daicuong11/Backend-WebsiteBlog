using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface ILoveRepository
    {
        Task<List<Love>> GetAll();
        Task<Love> GetLoveID(int id);
        Task<Love> GetLoveUserIDAndArticleID(int userId, int articleID);
        Task<Love> Create(Love love);
        Task UnLove(int id);
    }
}
