using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Entities;
using NewsWebAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Modals
{
    public record UserAuth
    {
        [Required(ErrorMessage = "Tên đăng nhập là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 100 ký tự.")]
        [MinLength(5, ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự.")]
        [MinLength(5, ErrorMessage = "Mật khẩu phải có ít nhất 5 ký tự.")]
        public string Password { get; set; }
    }
}
