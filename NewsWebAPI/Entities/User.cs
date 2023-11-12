using Microsoft.AspNetCore.Identity;
using NewsWebAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebAPI.Entities
{
    [Table("users")]
    public record User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "Họ và tên là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        [MinLength(2, ErrorMessage = "Tên phải có ít nhất 2 ký tự.")]
        public string Name { get; set; }

        [Column("username")]
        [Required(ErrorMessage = "Tên đăng nhập là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 100 ký tự.")]
        [MinLength(5, ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự.")]
        public string Username { get; set; }

        [Column("password")]
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }
        [Column("role")]
        public string Role { get; set; }

        //Khóa ngoại
        public List<Like>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }

    }
}
