using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface ICommentRepository
    {
        Task<Comment> AddComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(Comment comment);
        Task<List<Comment>> GetCommentByParentCommentID(int id);
        Task<List<Comment>> GetCommentByUserIdAndArticleId(int userId, int articleId);
    }
}
