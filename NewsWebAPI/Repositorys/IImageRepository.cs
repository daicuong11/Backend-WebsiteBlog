using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface IImageRepository
    {
        Task<List<Image>> GetAllImg();
        Task<Image> GetImageById(int id);
        Task<Image> CreateImg(Image img);
        Task<ImageArticleMapping> AddImgtoArticle(int articleId, ImageArticleMapping img);
        Task UpdateImg(Image img);
        Task DeleteImg(int id);
    }
}
