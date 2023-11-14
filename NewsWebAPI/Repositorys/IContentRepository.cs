using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface IContentRepository
    {
        Task<List<Content>> GetAllContents();
        Task<List<Content>> GetAllContentByArticleID(int articleId);
        Task<Content> GetContentById(int contentId);
        Task<Content> CreateContent(Content content);
        Task UpdateContent(Content content);
        Task DeleteContent(Content content);
    }
}
