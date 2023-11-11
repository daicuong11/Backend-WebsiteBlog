using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Api;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
using NewsWebAPI.MyExeption;
using NewsWebAPI.Repositorys;

namespace NewsWebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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
        
        [HttpPost()]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            try
            {
                user.Role = Role.GUEST.ToString();
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
                    User newUser = await _userRepository.Create(user);
                    var response = new MyResponse<User>(true, "Thêm thành công", newUser);
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
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id ,[FromBody] User user)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    var fError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                //    if (fError != null)
                //    {
                //        var error = fError.ErrorMessage;
                //        if (String.IsNullOrEmpty(error))
                //        {
                //            throw new ValidatorExeption(error);
                //        }
                //    }
                //}

                if (string.IsNullOrEmpty(user.Name))
                {
                    throw new ValidatorExeption("Tên không được để trống");
                }
                User getUserById = await _userRepository.GetById(id);
                if (getUserById != null)
                {
                    getUserById.Name = user.Name;

                    await _userRepository.Update(getUserById);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( [FromRoute] int id)
        {
            try
            {
                User getUserById = await _userRepository.GetById(id);
                if (getUserById != null)
                {
                    await _userRepository.Delete(getUserById);
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
    }
}
