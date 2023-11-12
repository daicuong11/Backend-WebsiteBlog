using Microsoft.AspNetCore.Identity;
using NewsWebAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    [Table("users")]
    public record User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Họ và tên là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        [MinLength(2, ErrorMessage = "Tên phải có ít nhất 2 ký tự.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 100 ký tự.")]
        [MinLength(5, ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        public string Role { get; set; }

        //Khóa ngoại
        [JsonIgnore]
        public List<Like>? Likes { get; set; }
        [JsonIgnore]
        public List<Comment>? Comments { get; set; }
        [JsonIgnore]
        public List<Article>? Articles { get; set; }

    }
}
