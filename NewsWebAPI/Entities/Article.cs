using NewsWebAPI.Modals;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class Article
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Tiêu đề là trường bắt buộc.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile Image {  get; set; }
        public string? ImagePath {  get; set; }
        public string Status { get; set; }

        public DateTime PublishDate { get; set; }
        public int? View { get; set; }
        //Khóa ngoại
        public int UserID { get; set; }
        public User User { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public List<Content> Contents { get; set; }

        public List<Love> Loves { get; set; }

    }
}
