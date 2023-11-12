using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebAPI.Entities
{
    public class Like
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("like_id")]
        public int likeId { get; set; }
        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        //khóa ngoại
        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public int Id { get; set; }
        public User? User { get; set; }
    }
}
