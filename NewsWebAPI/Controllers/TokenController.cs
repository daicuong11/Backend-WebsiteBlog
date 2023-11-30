using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsWebAPI.Api;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsWebAPI.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly MyDbContext _context;

        public TokenController(IConfiguration config, MyDbContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserAuth _userData)
        {
            if (_userData != null && _userData.Username != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(_userData.Password, user.Password))
                {
                    return Ok(new MyResponse<string>(false, "Sai tên tài khoản hoặc mật khẩu", ""));
                }

                // Tạo token và trả về response thành công
                var token = GenerateJwtToken(user);
                var respone = new
                {
                    user = new
                    {
                        UserID = user.UserID,
                        Name = user.Name,
                        Username = user.Username,
                        Role = user.Role,
                    },
                    token
                };
                return Ok(new MyResponse<Object>(true, "Đăng nhập thành công", respone));
            }
            else
            {
                return BadRequest(new MyResponse<string>(false, "Thiếu thông tin", ""));
            }
        }

        [Authorize] // Thêm attribute này để chỉ cho phép người dùng đã xác thực truy cập
        [HttpGet("info")]
        public IActionResult GetUserInfo()
        {
            try
            {
                // Lấy thông tin người dùng từ Claims
                var userId = User.FindFirst("UserId")?.Value;
                var name = User.FindFirst("Name")?.Value;
                var userName = User.FindFirst("UserName")?.Value;
                //var email = User.FindFirst("Email")?.Value;

                // Bạn có thể xử lý thông tin người dùng ở đây
                // Ví dụ: trả về thông tin người dùng dưới dạng JSON
                var userInfo = new
                {
                    UserID = userId,
                    Name = name,
                    Username = userName,
                    //Email = email,
                    Role = User.FindFirst("Role")?.Value
            };

                return Ok(new MyResponse<Object>(true, "Thông tin người dùng.", userInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(new MyResponse<string>(false, ex.Message, ""));
            }
        }

        private async Task<User> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.UserID.ToString()),
                new Claim("Name", user.Name),
                new Claim("UserName", user.Username),
                //new Claim("Email", user.Email),
                new Claim("Role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
