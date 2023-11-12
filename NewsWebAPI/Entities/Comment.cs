using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebAPI.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("comment_id")]
        public int CommentId { get; set; }
        [Column("content")]
        [Required(ErrorMessage = "Nội dung là trường bắt buộc.")]
        public string Content { get; set; }
        [Column("create_at")]
        public DateTime CreatedAt { get; set; }
        //Khóa ngoại
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public int Id { get; set; }
        public User? User { get; set; }
        public List<Comment>? Replies { get; set; }
    }
}
