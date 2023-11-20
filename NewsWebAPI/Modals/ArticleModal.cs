using NewsWebAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NewsWebAPI.Modals
{
    public class ArticleModal
    {
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Tiêu đề là trường bắt buộc.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string? ImagePath { get; set; }
        public string Status { get; set; }

        public DateTime PublishDate { get; set; }
        public int? View { get; set; }
        //Khóa ngoại
        public int UserID { get; set; }

        public int CategoryID { get; set; }

    }
}
