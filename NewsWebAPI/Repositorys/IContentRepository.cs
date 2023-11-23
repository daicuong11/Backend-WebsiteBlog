using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys
{
    public interface IContentRepository
    {
        Task<List<Content>> GetAllContents();
        Task<List<Content>> GetAllContentByArticleID(int articleId);
        Task<Content> GetContentById(int contentId);
        Task<Content> CreateContent(ContentModal content);
        Task UpdateContent(ContentModal content);
        Task DeleteContent(Content content);
    }
}
