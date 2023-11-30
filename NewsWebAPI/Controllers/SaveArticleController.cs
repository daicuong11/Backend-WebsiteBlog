using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;
using NewsWebAPI.Repositorys;
using NewsWebAPI.Repositorys.Services;

namespace NewsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveArticleController : ControllerBase
    {
        private readonly ISaveArticleRepository _saveArticleRepository;
        private readonly IMapper _mapper;

        public SaveArticleController(ISaveArticleRepository saveArticleRepository, IMapper mapper)
        {
            _saveArticleRepository = saveArticleRepository;
            _mapper = mapper;
        }

        [HttpGet("target")]
        public async Task<IActionResult> GetSavedByUserTarget([FromQuery] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                List<SavedArticle> counts = await _saveArticleRepository.GetAllByUserID(id);
                List<SavedArticle> saveds = await _saveArticleRepository.GetSavedOfUserTargetID(id, pageNumber, pageSize);

                var response = new PagedResponse<SavedArticle>(true, "Danh sách bài viết đã lưu", saveds, pageNumber, pageSize, counts.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaveArticleModal saveArticle)
        {
            try
            {
                var find = await _saveArticleRepository.GetSavedByUserIDAndArticleID(saveArticle.UserTargetID, saveArticle.ArticleID);
                if(find != null)
                {
                    await _saveArticleRepository.Delete(find);
                    var res = new MyResponse<SavedArticle>(true, "Hủy lưu thành công", null);
                    return Ok(res);
                }
                saveArticle.SavedAt = DateTime.Now;
                var newSaved = await _saveArticleRepository.Create(_mapper.Map<SavedArticle>(saveArticle));
                var response = new MyResponse<SavedArticle>(true, "Lưu thành công", newSaved);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }
    }
}
