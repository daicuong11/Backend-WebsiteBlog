using NewsWebAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsWebAPI.DTOs
{
    public class ArticleDTO
    {
        [Required(ErrorMessage = "Tiêu đề là trường bắt buộc.")]
        public string Title { get; set; }
        public string? ArticleContent { get; set; }
        public string? Image { get; set; }
        public string Status { get; set; }

        public DateTime PublishDate { get; set; }
        public int? View { get; set; }
        public string? Category { get; set; }
        public int UserID { get; set; }
    }
}
