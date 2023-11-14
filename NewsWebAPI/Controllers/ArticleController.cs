using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
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
        private readonly IArticleRepository _articleRepository;

        [ActivatorUtilitiesConstructor]
        public ArticleController(IArticleRepository articleRepository, IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Article> articles = await _articleRepository.GetAllArticles();
                var response = new MyResponse<List<Article>>(true, "Danh sách bài viết", articles);
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
                    var response = new MyResponse<Article>(false, "Không tim thấy bài viết với id = " + id, null);
                    return NotFound(response);
                }
                else {
                    var response = new MyResponse<Article>(false, "Bài viết với id = " + id, article);
                    return Ok(response);
                }
            }
            catch(Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromForm] Article article)
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

                // Kiểm tra và xử lý upload ảnh
                if (article.Image != null && article.Image.Length > 0)
                {
                    // Tạo đường dẫn và lưu ảnh vào thư mục
                    var imagePath = Path.Combine("resource", "img", Guid.NewGuid().ToString() + Path.GetExtension(article.Image.FileName));
                    var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await article.Image.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn vào thuộc tính ImagePath của bài viết
                    article.ImagePath = imagePath;
                }

                // Xữ lý khi user truyền vào thiếu thông tin (validate form)
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


        //[HttpPost]
        //public async Task<IActionResult> CreateArticle([FromBody] Article article)
        //{
        //    try
        //    {
        //        User findUserById = await _userRepository.GetById(article.UserID);
        //        if(findUserById == null)
        //        {
        //            var res = new MyResponse<Article>(false, "Không tim thấy tác giả với id = " + article.UserID, null);
        //            return NotFound(res);
        //        }
        //        article.PublishDate = DateTime.Now;
        //        //Xữ lý khi user truyền vào thiếu thông tin (validate form)
        //        if (!ModelState.IsValid)
        //        {
        //            var fError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
        //            if (fError != null)
        //            {
        //                var error = fError.ErrorMessage;
        //                if (String.IsNullOrEmpty(error))
        //                {
        //                    throw new ValidatorExeption(error);
        //                }
        //            }
        //        }

        //        Article newArticle = await _articleRepository.CreateArticle(article);
        //        var response = new MyResponse<Article>(true, "Thêm thành công", newArticle);
        //        return StatusCode(201, response);
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new MyResponse<string>(false, "Server error 500", ex.Message);
        //        return BadRequest(response);
        //    }
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Article article)
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
                        // Tạo đường dẫn và lưu ảnh vào thư mục
                        var imagePath = Path.Combine("resource", "img", Guid.NewGuid().ToString() + Path.GetExtension(article.Image.FileName));
                        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await article.Image.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn vào thuộc tính ImagePath của bài viết
                        article.ImagePath = imagePath;
                    }
                    findArticleById.Title = article.Title;
                    findArticleById.Description = article.Description;
                    findArticleById.ImagePath = article.ImagePath;

                    await _articleRepository.UpdateArticle(findArticleById);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Article findArticleById = await _articleRepository.GetArticleById(id);
                if (findArticleById == null)
                {
                    var response = new MyResponse<Article>(false, "Không tìm thấy bài viết nào với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    await _articleRepository.DeleteArticle(findArticleById);
                    var response = new MyResponse<Article>(true, "Xóa thành công", null);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("likes/{id}")]
        public async Task<IActionResult> GetLikeOfArticle([FromRoute] int id)
        {
            try
            {
                Article findArticleById = await _articleRepository.GetArticleById(id);
                if(findArticleById == null)
                {
                    var response = new MyResponse<Article>(false, "Không tìm thấy bài viết nào với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    List<Like> likes = await _articleRepository.GetLikesForArticle(id);
                    var response = new MyResponse<List<Like>>(true, "Danh sách lượi thích", likes);
                    return Ok(response);
                }
            }
            catch(Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        //[HttpGet("images/{imageName}")]
        //public IActionResult GetImage(string imageName)
        //{
        //    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resource", "img", imageName);
        //    // Kiểm tra xem ảnh có tồn tại không
        //    if (System.IO.File.Exists(imagePath))
        //    {
        //        // Đọc dữ liệu từ tệp và trả về nó dưới dạng nội dung đáp ứng
        //        var imageFileStream = System.IO.File.OpenRead(imagePath);
        //        return File(imageFileStream, "image/jpeg"); // Thay đổi loại MIME tùy thuộc vào loại ảnh bạn đang sử dụng
        //    }

        //    // Trả về lỗi nếu ảnh không tồn tại
        //    var response = new MyResponse<string>(false, "Không tìm thấy ảnh với tên " + imageName, "");
        //    return NotFound(response);
        //}

    }
}
