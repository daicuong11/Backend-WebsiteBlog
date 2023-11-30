using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface ISaveArticleRepository
    {
        Task<List<SavedArticle>> GetAllByUserID(int id);
        Task<List<SavedArticle>> GetSavedOfUserTargetID(int id, int pageNumber, int pageSize);
        Task<SavedArticle> GetSavedByUserIDAndArticleID(int userID, int articleID);
        Task<SavedArticle> Create(SavedArticle savedArticle);
        Task Delete(SavedArticle savedArticle);
    }
}
