using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.MyExeption;
using NewsWebAPI.Repositorys;
using NewsWebAPI.Repositorys.Services;
using System.ComponentModel.DataAnnotations;

namespace NewsWebAPI.Controllers
{
    [Route("api/content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentRepository _contentRepository;
        private readonly IArticleRepository _articleRepository;

        [ActivatorUtilitiesConstructor]
        public ContentController(IContentRepository contentRepository, IArticleRepository articleRepository)
        {
            _contentRepository = contentRepository;
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContents()
        {
            try
            {
                List<Content> contents = await _contentRepository.GetAllContents();
                var response = new MyResponse<List<Content>>(true, "Danh sách nội dung", contents);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById([FromRoute] int id)
        {
            try
            {
                Content content = await _contentRepository.GetContentById(id);
                if (content == null)
                {
                    var response = new MyResponse<Content>(false, "Không tìm thấy nội dung với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    var response = new MyResponse<Content>(true, "Nội dung với id = " + id, content);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetAllContentByArticleID([FromRoute] int articleId)
        {
            try
            {
                List<Content> contents = await _contentRepository.GetAllContentByArticleID(articleId);
                var response = new MyResponse<List<Content>>(true, "Danh sách nội dung theo bài viết", contents);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromForm] Content content)
        {
            try
            {
                Article findArticleById = await _articleRepository.GetArticleById(content.ArticleID);
                if (findArticleById == null)
                {
                    var res = new MyResponse<Article>(false, "Không tìm thấy bài viết với id = " + content.ArticleID, null);
                    return NotFound(res);
                }

                // Kiểm tra và xử lý upload ảnh
                if (content.ContentImage != null && content.ContentImage.Length > 0)
                {
                    // Tạo đường dẫn thư mục
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resource", "img");

                    // Kiểm tra nếu thư mục không tồn tại, tạo mới
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Tạo đường dẫn và lưu ảnh vào thư mục với tên duy nhất
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(content.ContentImage.FileName);
                    var imagePath = Path.Combine("resource", "img", imageName);
                    var physicalPath = Path.Combine(directoryPath, imageName);

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await content.ContentImage.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn vào thuộc tính ContentImagePath của nội dung
                    content.ContentImagePath = imagePath;
                }

                // Xữ lý khi truyền vào thiếu thông tin (validate form)
                if (!ModelState.IsValid)
                {
                    var fError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                    if (fError != null)
                    {
                        var error = fError.ErrorMessage;
                        if (String.IsNullOrEmpty(error))
                        {
                            throw new ValidationException(error);
                        }
                    }
                }

                Content newContent = await _contentRepository.CreateContent(content);
                var response = new MyResponse<Content>(true, "Thêm nội dung thành công", newContent);
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent([FromRoute] int id, [FromBody] Content content)
        {
            try
            {
                Content findContentById = await _contentRepository.GetContentById(id);
                if (findContentById == null)
                {
                    var response = new MyResponse<Content>(false, "Không tìm thấy nội dung nào với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    // Kiểm tra và xử lý upload ảnh
                    if (content.ContentImage != null && content.ContentImage.Length > 0)
                    {
                        // Tạo đường dẫn thư mục
                        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resource", "img");

                        // Kiểm tra nếu thư mục không tồn tại, tạo mới
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        // Tạo đường dẫn và lưu ảnh vào thư mục
                        var imagePath = Path.Combine("resource", "img", Guid.NewGuid().ToString() + Path.GetExtension(content.ContentImage.FileName));
                        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await content.ContentImage.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn vào thuộc tính ImagePath của bài viết
                        content.ContentImagePath = imagePath;
                    }
                    findContentById.ContentTitle = content.ContentTitle;
                    findContentById.ContentBody = content.ContentBody;
                    findContentById.ContentImagePath = content.ContentImagePath;

                    await _contentRepository.UpdateContent(findContentById);
                    var response = new MyResponse<string>(true, "Cập nhật nội dung thành công", null);
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
        public async Task<IActionResult> DeleteContent([FromRoute] int id)
        {
            try
            {
                Content findContentByContentID = await _contentRepository.GetContentById(id);
                if (findContentByContentID == null)
                {
                    var response = new MyResponse<Article>(false, "Không tìm thấy bài viết nào với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    await _contentRepository.DeleteContent(findContentByContentID);
                    var response = new MyResponse<Content>(true, "Xóa nội dung thành công", null);
                    return Ok(response);
                }
            }
            catch (Exception ex)
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
