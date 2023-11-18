using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Repositorys;
namespace NewsWebAPI.Controllers
{
    [Route("api/Categorys")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly IArticleRepository _articleRepository;
        public readonly ICategoryRepository _CategoryRepository;
        public static IWebHostEnvironment _environment;

        [ActivatorUtilitiesConstructor]
        public CategoryController(IArticleRepository articleRepository, ICategoryRepository CategoryRepository, IWebHostEnvironment environment)
        {
            _articleRepository = articleRepository;
            _CategoryRepository = CategoryRepository;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? s = "", int pageNumber = 1, int pageSize = 2, string sortOrder = "")
        {
            try
            {
                List<Category> imgs = await _CategoryRepository.GetAll(s,pageNumber, pageSize, sortOrder);
                var response = new PaginateResponse<List<Category>>(true, "List category", imgs,pageNumber,pageSize);
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
                Category category = await _CategoryRepository.FindOne(id);
                if (category != null)
                {
                    var response = new MyResponse<Category>(true, "Category with id = " + id, category);
                    return Ok(response);
                }
                else
                {
                    var response = new MyResponse<Category>(false, "None Category" + id, null);
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
        public async Task<IActionResult> Create([FromBody] Category img)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    var errorMessage = string.Join(", ", errors);
                    return BadRequest(new MyResponse<string>(false, "Invalid data", errorMessage));
                }

                if (img == null)
                {
                    return BadRequest(new MyResponse<string>(false, "Invalid data", "Object is null"));
                }

                var addedCategory = await _CategoryRepository.Create(img);

                if (addedCategory == null)
                {
                    return BadRequest(new MyResponse<string>(false, "Error creating Category", "Không thể tạo Category"));
                }

                return Ok(new MyResponse<Category>(true, "Category đã được tạo", addedCategory));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, "Error creating Category", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            try
            {
                var existingCategory = await _CategoryRepository.FindOne(id);
                if (existingCategory == null)
                {
                    return NotFound(new MyResponse<string>(false, "Không tìm thấy Category", "Category not found"));
                }
                existingCategory.CategoryName = category.CategoryName;
                await _CategoryRepository.Update(existingCategory);
                
                return Ok(new MyResponse<Category>(true, "Category đã được cập nhật", existingCategory));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, "Lỗi khi cập nhật Category", ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existCategory = await _CategoryRepository.FindOne(id);
                if (existCategory == null)
                {
                    return NotFound(new MyResponse<string>(false, "Không tìm thấy Category", "Category not found"));
                }
                await _CategoryRepository.Delete(existCategory);
                return Ok(new MyResponse<string>(true, "Category đã được xóa", "Category deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, "Lỗi khi xóa Category", ex.Message));
            }
        }
    }
}
