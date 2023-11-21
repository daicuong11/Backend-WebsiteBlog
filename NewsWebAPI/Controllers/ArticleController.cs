using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
using NewsWebAPI.Modals;
using NewsWebAPI.MyExeption;
using NewsWebAPI.Repositorys;
using System.Globalization;

namespace NewsWebAPI.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;

        [ActivatorUtilitiesConstructor]
        public ArticleController(IArticleRepository articleRepository, IUserRepository userRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedArticles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                List<Article> articleAll = await _articleRepository.GetAllArticles();
                List<Article> articles = await _articleRepository.GetPagedArticles(pageNumber, pageSize);
                var response = new PagedResponse<Article>(true, "List of articles", articles, pageNumber, pageSize, articleAll.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                Article article = await _articleRepository.GetArticleById(id);
                if(article == null)
                {
                    var response = new MyResponse<ArticleModal>(false, "Không tim thấy bài viết với id = " + id, null);
                    return NotFound(response);
                }
                else {
                    var response = new MyResponse<Article>(true, "Bài viết với id = " + id, article);
                    return Ok(response);
                }
            }
            catch(Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }


        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetPagedArticlesByCategoryID([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                List<Article> articleAllByCategoryID = await _articleRepository.GetAllArticlesByCategoryID(id);
                List<Article> articles = await _articleRepository.GetPagedArticlesByCategoryID(id, pageNumber, pageSize);
                var response = new PagedResponse<Article>(true, $"List of articles in category {id}", articles, pageNumber, pageSize, articleAllByCategoryID.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetPagedArticlesByUserID([FromRoute] int id)
        {
            try
            {
                List<Article> articles = await _articleRepository.GetAllArticlesByUserID(id);
                var response = new MyResponse<List<Article>>(true, $"List of articles in category {id}", articles);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleModal article)
        {
            try
            {
                User findUserById = await _userRepository.GetById(article.UserID);
                if (findUserById == null)
                {
                    var res = new MyResponse<Article>(false, "Không tim thấy tác giả với id = " + article.UserID, null);
                    return NotFound(res);
                }

                article.PublishDate = DateTime.Now;

                if (article.Image != null && article.Image.Length > 0)
                {
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resource", "img");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(article.Image.FileName);
                    var imagePath = Path.Combine("resource", "img", imageName);
                    var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await article.Image.CopyToAsync(stream);
                    }

                    article.ImagePath = imageName;
                }

                if (!ModelState.IsValid)
                {
                    var fError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                    if (fError != null)
                    {
                        var error = fError.ErrorMessage;
                        if (String.IsNullOrEmpty(error))
                        {
                            throw new ValidatorExeption(error);
                        }
                    }
                }

                Article newArticle = await _articleRepository.CreateArticle(article);
                var response = new MyResponse<Article>(true, "Thêm thành công", newArticle);
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ArticleModal article)
        {
            try
            {
                //Xữ lý khi user truyền vào thiếu thông tin (validate form)
                if (String.IsNullOrEmpty(article.Title.Trim()))
                {
                    throw new ValidatorExeption("Tiêu đề không được để trống");
                }
                Article findArticleById = await _articleRepository.GetArticleById(id);
                if(findArticleById == null)
                {
                    var response = new MyResponse<Article>(false, "Không tìm thấy bài viết nào với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    if (article.Image != null && article.Image.Length > 0)
                    {
                        // Tạo đường dẫn thư mục
                        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resource", "img");

                        // Kiểm tra nếu thư mục không tồn tại, tạo mới
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        // Tạo đường dẫn và lưu ảnh vào thư mục
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(article.Image.FileName);
                        var imagePath = Path.Combine("resource", "img", imageName);
                        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await article.Image.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn vào thuộc tính ImagePath của bài viết
                        article.ImagePath = imageName;
                    }
                    findArticleById.Title = article.Title;
                    findArticleById.Description = article.Description;
                    findArticleById.ImagePath = article.ImagePath;
                    

                    await _articleRepository.UpdateArticle(_mapper.Map<ArticleModal>(findArticleById));
                    var response = new MyResponse<string>(true, "Cập nhật thành công", null);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateArticleStatus([FromRoute] int id, [FromBody] ArticleStatusModal statusCode)
        {
            try
            {
                Article findArticleByID = await _articleRepository.GetArticleById(id);
                if(findArticleByID == null)
                {
                    var res = new MyResponse<string>(false, "Không tìm thấy articleID = " + id, "");
                    return NotFound(res);
                }
                else if (statusCode.StatusCode < 1 || statusCode.StatusCode > 5)
                {
                    var res = new MyResponse<string>(false, "Không có statusCode = " + statusCode.StatusCode, "");
                    return NotFound(res);
                }
                await _articleRepository.UpdateArticleStatus(id, statusCode.StatusCode);
                var response = new MyResponse<string>(true, "Sửa thành công trạng thái statusCode = " + statusCode.StatusCode, "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Article findArticleById = await _articleRepository.GetArticleById(id);
                if (findArticleById == null)
                {
                    var response = new MyResponse<ArticleModal>(false, "Không tìm thấy bài viết nào với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    await _articleRepository.DeleteArticle(_mapper.Map<ArticleModal>(findArticleById));
                    var response = new MyResponse<ArticleModal>(true, "Xóa thành công", null);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

    }
}
