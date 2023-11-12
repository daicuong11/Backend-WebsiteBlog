using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class Like
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int likeID { get; set; }
        public DateTime CreateAt { get; set; }

        //khóa ngoại
        [Required(ErrorMessage = "Mã bài viết là trường bắt buộc.")]
        public int ArticleID { get; set; }
        [JsonIgnore]
        public Article? Article { get; set; }
        [Required(ErrorMessage = "Mã tác giả là trường bắt buộc.")]
        public int UserID { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
