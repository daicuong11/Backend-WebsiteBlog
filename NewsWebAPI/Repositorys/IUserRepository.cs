using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetById(int id);

        Task<User> Create(User user);
        Task Update(User user);
        Task Delete(User user);

        Task<User> FindByUserName(string userName);
        Task<User> FindByPassWord(string passWord);
    }
}
