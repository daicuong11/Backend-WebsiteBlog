using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
using NewsWebAPI.MyExeption;
using NewsWebAPI.Repositorys;
using NewsWebAPI.Repositorys.Services;
using System.Globalization;

namespace NewsWebAPI.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public readonly IArticleRepository _articleRepository;
        public readonly IImageRepository _imageRepository;

        [ActivatorUtilitiesConstructor]
        public ImageController(IArticleRepository articleRepository, IImageRepository imageRepository)
        {
            _articleRepository = articleRepository;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Image> imgs = await _imageRepository.GetAllImg();
                var response = new MyResponse<List<Image>>(true, "Danh sách các ảnh", imgs);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImgById([FromRoute] int id)
        {
            try
            {
                Image img = await _imageRepository.GetImageById(id);
                if (img != null)
                {
                    var response = new MyResponse<Image>(false, "Bài viết với id = " + id, img);
                    return Ok(response);
                }
                else
                {
                    var response = new MyResponse<Image>(false, "Không tim thấy ảnh với" + id, null);
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage([FromBody] Image img)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    var errorMessage = string.Join(", ", errors);
                    return BadRequest(new MyResponse<string>(false, "Dữ liệu không hợp lệ", errorMessage));
                }

                if (img == null)
                {
                    return BadRequest(new MyResponse<string>(false, "Dữ liệu không hợp lệ", "Object is null"));
                }

                var addedImage = await _imageRepository.CreateImg(img);

                if (addedImage == null)
                {
                    return BadRequest(new MyResponse<string>(false, "Lỗi khi tạo hình ảnh", "Không thể tạo hình ảnh"));
                }

                return Ok(new MyResponse<Image>(true, "Hình ảnh đã được tạo", addedImage));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, "Lỗi khi tạo hình ảnh", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromBody] Image img)
        {
            try
            {
                if (img == null || id != img.ImageID)
                {
                    return BadRequest(new MyResponse<string>(false, "Dữ liệu không hợp lệ", "Invalid data or ID mismatch"));
                }

                var existingImage = await _imageRepository.GetImageById(id);
                if (existingImage == null)
                {
                    return NotFound(new MyResponse<string>(false, "Không tìm thấy hình ảnh", "Image not found"));
                }

                // Update image properties here
                // For example: existingImage.Property = img.Property;

                await _imageRepository.UpdateImg(existingImage);

                return Ok(new MyResponse<Image>(true, "Hình ảnh đã được cập nhật", existingImage));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, "Lỗi khi cập nhật hình ảnh", ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                var existImage = await _imageRepository.GetImageById(id);
                if (existImage == null)
                {
                    return NotFound(new MyResponse<string>(false, "Không tìm thấy hình ảnh", "Image not found"));
                }
                await _imageRepository.DeleteImg(id);
                return Ok(new MyResponse<string>(true, "Hình ảnh đã được xóa", "Image deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, "Lỗi khi xóa hình ảnh", ex.Message));
            }
        }
    }
}
