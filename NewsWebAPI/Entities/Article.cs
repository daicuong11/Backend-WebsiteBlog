using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebAPI.Entities
{
    public record Article
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("article_id")]
        public int ArticleId { get; set; }
        [Column("title")]
        [Required(ErrorMessage = "Tiêu đề là trường bắt buộc.")]
        public string Title { get; set; }
        [Column("content")]
        public string? content { get; set; }
        [Column("publish_date")]

        public DateTime publishDate { get; set; }
        [Column("view")]
        public int? View { get; set; }
        [Column("category")]
        public string? Category { get; set; }
        //Khóa ngoại
        public List<Like>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }

    }
}
