using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<int> Count();
        Task<List<User>> GetAllPagination(string? searchTerm, int pageNumber, int pageSize, string sortOrder);

        Task<User> GetById(int id);

        Task<User> Create(UserModal user);
        Task Update(UserModal user);
        Task Delete(UserModal user);

        Task<User> FindByUserName(string userName);
        Task<User> FindByPassWord(string passWord);

        Task ChangePassword(User user);

    }
}
