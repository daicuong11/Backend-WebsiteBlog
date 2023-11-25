using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Api;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;
using NewsWebAPI.Repositorys;

namespace NewsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoveController : ControllerBase
    {
        private ILoveRepository _loveRepository;
        private IMapper _mapper;

        public LoveController(ILoveRepository loveRepository, IMapper mapper)
        {
            _loveRepository = loveRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Love> loves = await _loveRepository.GetAll();
                var response = new MyResponse<List<Love>>(true, "Danh sách yêu thích", loves);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoveModal love)
        {
            try
            {
                var findLove = await _loveRepository.GetLoveUserIDAndArticleID(love.UserTargetID, love.ArticleID);
                if(findLove != null)
                {
                    await _loveRepository.UnLove(findLove.LoveID);
                    return Ok(new MyResponse<bool>(true, "Bỏ thích", false));
                }
                var newLove = await _loveRepository.Create(_mapper.Map<Love>(love));
                var response = new MyResponse<bool>(true, "Tạo lượt thích thành công", true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpDelete("unlove")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                var love = await _loveRepository.GetLoveID(id);
                if (love == null)
                {
                    var res = new MyResponse<string>(false, "Không tìm thấy yêu thích id =" + id, "");
                    return NotFound(res);
                }
                await _loveRepository.UnLove(id);
                var response = new MyResponse<string>(true, "Bỏ thích", "");
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
