using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        [Required(ErrorMessage = "Nội dung là trường bắt buộc.")]
        public string CommentContent { get; set; }
        public DateTime CreatedAt { get; set; }
        //Khóa ngoại
        public int UserID { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public int ArticleID { get; set; }
        [JsonIgnore]
        public Article? Article { get; set; }

        public int? ParentCommentID { get; set; }
        public Comment? ParentComment { get; set; }
        public List<Comment>? Replies { get; set; }
    }
}
