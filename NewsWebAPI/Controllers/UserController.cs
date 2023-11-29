using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Api;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
using NewsWebAPI.Modals;
using NewsWebAPI.MyExeption;
using NewsWebAPI.Repositorys;
using NewsWebAPI.Repositorys.Services;

namespace NewsWebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        //[Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<User> users = await _userRepository.GetAll();
                var response = new MyResponse<List<User>>( true, "Danh sách người dùng", users);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest();
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetAllPagination(string? s = "", int pageNumber = 1, int pageSize = 2, string? sortOrder = "id")
        {
            try
            {
                List<User> users = await _userRepository.GetAllPagination(s, pageNumber, pageSize, (string)sortOrder);
                var response = new PaginateResponse<List<User>>(true, "Danh sách người dùng", users, pageNumber, pageSize, await _userRepository.Count());
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest();
            }
        }
        //[Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById( [FromRoute] int id) 
        {
            try
            {
                User user = await _userRepository.GetById(id);
                if(user  == null)
                {
                    var response = new MyResponse<User>( false, "Không tìm thấy người dùng với id = " + id, null);
                    return NotFound(response);
                }
                else
                {
                    var response = new MyResponse<User>(true, "Lấy người dùng với id = " + id, user);
                    return Ok(response);
                }   
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest();
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost()]
        public async Task<ActionResult> Create([FromBody] UserModal user)
        {
            try
            {
                if (user.Role != Role.GUEST.ToString() && user.Role != Role.AUTHOR.ToString() && user.Role != Role.ADMIN.ToString() && user.Role != Role.EDITOR.ToString()) {
                    return BadRequest(new MyResponse<string>(false, "Sai Role", ""));
                }
                //Xữ lý khi user truyền vào thiếu thông tin (validate form)
                if (!ModelState.IsValid)
                {
                    var fError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                    if(fError != null)
                    {
                        var error = fError.ErrorMessage;
                        if (String.IsNullOrEmpty(error))
                        {
                            throw new ValidatorExeption(error);
                        }
                    }
                }
                User findUserByUsername = await _userRepository.FindByUserName(user.Username);
                if (findUserByUsername == null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.CreateAt = DateTime.Now;
                    User newUser = await _userRepository.Create(user);
                    var response = new MyResponse<User>(true, "Đăng ký tài khoản thành công", newUser);
                    return StatusCode(201, response);
                }
                else
                {
                    var response = new MyResponse<User>(false, "Tên tài khoản đã tồn tại", null);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest(response);
            }
        }
        [Authorize(Policy = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id ,[FromBody] UserModal user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Name))
                {
                    throw new ValidatorExeption("Tên không được để trống");
                }
                User getUserById = await _userRepository.GetById(id);
                if (getUserById != null)
                {
                    getUserById.Name = user.Name;
                    getUserById.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    getUserById.Role = user.Role;
                    getUserById.IsLocked = user.IsLocked;
                    await _userRepository.Update(_mapper.Map<UserModal>(getUserById));
                    var response = new MyResponse<string>(true, "Cập nhật thành công", null);
                    return Ok(response);
                }
                else
                {
                    var response = new MyResponse<string>(false, "Không tìm thấy người dùng với id " + id, "");
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest(response);
            }
        }
        [Authorize(Policy = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( [FromRoute] int id)
        {
            try
            {
                User getUserById = await _userRepository.GetById(id);
                if (getUserById != null)
                {
                    await _userRepository.Delete(_mapper.Map<UserModal>(getUserById));
                    var response = new MyResponse<string>(true, "Xóa thành công", null);
                    return Ok(response);
                }
                else
                {
                    var response = new MyResponse<string>(false, "Không tìm thấy người dùng với id = " + id, null);
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest();
            }
        }

        [HttpPost("lock/{id}")]
        public async Task<ActionResult> Lock ([FromRoute] int id)
        {
            try
            {
                User userToLock = await _userRepository.GetById(id);
                if (userToLock != null)
                {
                    userToLock.IsLocked = !userToLock.IsLocked;
                    await _userRepository.Update(_mapper.Map<UserModal>(userToLock));
                    var response = new MyResponse<string>(true, $"User has been locked", "");
                    return Ok(response);
                } else
                {
                    var response = new MyResponse<string>(false, $"User not found", "");
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new MyResponse<string>(false, ex.Message, "");
                return BadRequest(response);
            }
        }
    }
}
