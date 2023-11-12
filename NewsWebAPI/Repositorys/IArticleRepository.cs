using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface IArticleRepository
    {
        Task<Article> GetArticleById(int id);
        Task<List<Article>> GetAllArticles();
        Task<List<Like>> GetLikesForArticle(int articleId);
        Task<List<Comment>> GetCommentsForArticle(int articleId);
        Task<Comment> GetCommentById(int commentId);
        Task<Comment> AddComment(Comment comment);
        Task<Like> AddLike(Like like);

        // Phương thức CRUD cho Article
        Task<Article> CreateArticle(Article article);
        Task UpdateArticle(Article article);
        Task DeleteArticle(Article article);
    }

}
