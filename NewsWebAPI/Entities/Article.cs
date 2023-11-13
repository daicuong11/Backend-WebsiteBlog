using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public record Article
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Tiêu đề là trường bắt buộc.")]
        public string Title { get; set; }
        public string? ArticleContent { get; set; }
        public string? Image {  get; set; }
        public string Status { get; set; }

        public DateTime PublishDate { get; set; }
        public int? View { get; set; }
        //Khóa ngoại
        public int UserID { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public List<Like>? Likes { get; set; }
        [JsonIgnore]
        public List<Comment>? Comments { get; set; }

        public int CategoryID { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }

    }
}
