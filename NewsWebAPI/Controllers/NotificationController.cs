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
    public class NotificationController : ControllerBase
    {
        private INotificationRepository _notificationRepository;
        private IMapper _mapper;

        public NotificationController(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        [HttpGet("userID")]
        public async Task<IActionResult> GetNotificationByUserUD([FromQuery] int id)
        {
            try
            {
                List<Notification> notis = await _notificationRepository.GetNotifiOfUserCreateID(id);
                var response = new MyResponse<List<Notification>>(true, "Danh hoạt động của bạn", notis);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("target")]
        public async Task<IActionResult> GetNotificationByUserTarget([FromQuery] int id) 
        {
            try
            {
                List<Notification> notis = await _notificationRepository.GetNotifiOfUserTargetID(id);
                var response = new MyResponse<List<Notification>>(true, "Danh sách thông báo", notis);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationModal notify)
        {
            try
            {
                notify.CreatedAt = DateTime.Now;
                notify.IsRead = false;
                var newNotify = await _notificationRepository.Create(_mapper.Map<Notification>(notify));
                var response = new MyResponse<Notification>(true, "Tạo thành công thông báo", newNotify);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, "Server error 500", ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("isRead")]
        public async Task<IActionResult> Create([FromQuery] int id)
        {
            try
            {
                var notify = await _notificationRepository.GetNotifiByID(id);
                if(notify == null)
                {
                    var res = new MyResponse<string>(false, "Không tìm thấy thông báo id =" + id, "");
                    return NotFound(res);
                }
                await _notificationRepository.IsRead(id);
                var response = new MyResponse<string>(true, "Đã đọc thông báo", "");
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
