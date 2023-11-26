using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys
{
    public interface IArticleRepository
    {
        Task<Article> GetArticleById(int id);
        Task<List<Article>> GetAllArticles();
        Task<List<Article>> GetAllArticlesByCategoryID(int id);
        Task<List<Article>> GetAllArticlesByUserID(int id);
        // Phương thức CRUD cho Article
        Task<Article> CreateArticle(ArticleModal article);
        Task UpdateArticle(ArticleModal article);
        Task UpdateArticleStatus(int id, int statusCode);
        Task DeleteArticle(ArticleModal article);

        // Phương thức hỗ trợ phân trang
        Task<List<Article>> GetPagedArticles(int pageNumber, int pageSize);
        Task<List<Article>> GetPagedArticlesByCategoryID(int id, int pageNumber, int pageSize);

        Task<List<Article>> GetArticlesBySearchKey(string searchKey);

        Task<List<Article>> GetArticlesLatest(int pageNumber, int pageSize);
        Task<List<Article>> GetArticlesFavourite(int pageNumber, int pageSize);
        Task<List<Article>> GetTop3ArticlesFavouriteByUserID(int id);
        Task<List<Article>> GetRandomArticles(int pageNumber, int pageSize);
    }
}
