using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface ILikeRepository
    {
        Task<Like> Like(Like item);
        Task UnLike(Like like);
        Task<Like> GetLikeByUserIdAndArticleId(int userId, int articleId);
    }
}
