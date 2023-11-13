using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class ImageRepository : IImageRepository
    {
        public readonly MyDbContext _dbContext;
        public ImageRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Image>> GetAllImg()
        {
            var allImages = await _dbContext.Images.ToListAsync();
            return allImages;
        }

        public async Task<ImageArticleMapping> AddImgtoArticle(int articleId, ImageArticleMapping img)
        {
            //Kiểm tra xem articleId có tồn tại trong bả ng Article không
            var article = await _dbContext.Articles.FindAsync(articleId);
            if (article == null)
            {
                return null;
            }
            img.ArticleID = articleId;
            var imgs = await _dbContext.ImageArticleMappings.AddAsync(img);
            await _dbContext.SaveChangesAsync();
            return imgs.Entity;
        }
        public async Task<Image> CreateImg(Image img)
        {
            var addedImage = await _dbContext.Images.AddAsync(img);
            await _dbContext.SaveChangesAsync();
            return addedImage.Entity;
        }
        public async Task DeleteImg(int id)
        {
            var img = await GetImageById(id);
            if (img != null)
            {
                _dbContext.Images.Remove(img);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Image> GetImageById(int id)
        {
            return await _dbContext.Images.FindAsync(id);
        }

        public async Task<List<ImageArticleMapping>> GetImagesForArticle(int articleId)
        {
            var imgsForArticle = _dbContext.ImageArticleMappings.Where(mapping => mapping.ArticleID == articleId).ToListAsync();
            return await imgsForArticle;
        }

        public async Task UpdateImg(Image img)
        {
            _dbContext.Images.Update(img);
            await _dbContext.SaveChangesAsync();
        }
    }
}
