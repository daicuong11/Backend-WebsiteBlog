using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetById(int id);

        Task<User> Create(UserModal user);
        Task Update(UserModal user);
        Task Delete(UserModal user);

        Task<User> FindByUserName(string userName);
        Task<User> FindByPassWord(string passWord);
    }
}
