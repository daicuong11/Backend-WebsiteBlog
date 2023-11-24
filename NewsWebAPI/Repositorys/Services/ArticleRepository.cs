using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ArticleRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(a => a.ArticleID == id);
        }

        public async Task<List<Article>> GetAllArticles()
        {
            return await _context.Articles
                .Include(a => a.User)
                .ToListAsync();
        }

        //phân trang
        public async Task<List<Article>> GetPagedArticles(int pageNumber, int pageSize)
        {
            if(pageSize == 0)
            {
                return await _context.Articles
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .ToListAsync();
            }
            return await _context.Articles
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Article>> GetPagedArticlesByCategoryID(int id, int pageNumber, int pageSize)
        {
            return await _context.Articles
                .Where(a => a.CategoryID == id)
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Article>> GetAllArticlesByUserID(int id)
        {
            return await _context.Articles
                .Where(a => a.UserID == id)
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .ToListAsync();
        }

        public async Task<Article> CreateArticle(ArticleModal article)
        {
            var newArticle = _mapper.Map<Article>(article);
            var entityEntry = await _context.Articles.AddAsync(newArticle);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task UpdateArticle(ArticleModal article)
        {
            var newArticle = _mapper.Map<Article>(article);
            _context.Articles!.Update(newArticle);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateArticleStatus(int id, int statusCode)
        {
            var findArticleByID = await _context.Articles.FindAsync(id);
            if (findArticleByID != null)
            {
                string status;
                //DRAFT 1
                //PENDING 2
                //PUBLISHED 3
                //DELETED 4
                //REJECTED 5
                if (statusCode == 1) 
                {
                    status = ArticleStatus.DRAFT.ToString();
                }
                else if (statusCode == 2)
                {
                    status = ArticleStatus.PENDING.ToString();
                }
                else if (statusCode == 3)
                {
                    status = ArticleStatus.PUBLISHED.ToString();
                }
                else if (statusCode == 4)
                {
                    status = ArticleStatus.DELETED.ToString();
                }
                else if (statusCode == 5)
                {
                    status = ArticleStatus.REJECTED.ToString();
                }
                else
                {
                    return;
                }
                findArticleByID.Status = status;
                _context.Articles?.Update(findArticleByID);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteArticle(ArticleModal article)
        {
            var newArticle = _mapper.Map<Article>(article);
            _context.Articles.Remove(newArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Article>> GetAllArticlesByCategoryID(int id)
        {
            return await _context.Articles.Where(a =>  a.CategoryID == id).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesBySearchKey(string searchKey)
        {
            return await _context.Articles
                .Where(article => article.Title.Contains(searchKey) || article.Description.Contains(searchKey))
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesLatest(int pageNumber, int pageSize)
        {
            return await _context.Articles
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .OrderByDescending(a => a.PublishDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesFavourite(int pageNumber, int pageSize)
        {
            // Giả sử có một trường trong bảng Article để xác định các bài viết yêu thích, ví dụ: IsFavourite
            return await _context.Articles
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .OrderByDescending(a => a.Loves.Count)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Article>> GetRandomArticles(int pageNumber, int pageSize)
        {
            var totalArticles = await _context.Articles.CountAsync();

            // Truy vấn cơ sở dữ liệu để lấy các bài viết với sắp xếp ngẫu nhiên
            return await _context.Articles
                .Include(a => a.Loves)
                .Include(a => a.User)
                .Include(a => a.Contents)
                .Include(a => a.Category)
                .OrderBy(a => Guid.NewGuid())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
