using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsWebAPI.Entities
{
    public class SavedArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SavedArticleID { get; set; }

        public DateTime? SavedAt { get; set; }
        public int UserTargetID { get; set; }
        public int ArticleID { get; set; }
        public Article Article { get; set; }
    }
}
