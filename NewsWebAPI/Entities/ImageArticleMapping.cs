using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class ImageArticleMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageArticleMappingID { get; set; }
        // khóa ngoại
        public int ArticleID { get; set; }
        [JsonIgnore]
        public Article? Article { get; set; }

        public int ImageID { get; set; }
        [JsonIgnore]
        public Image? Image { get; set; }
    }
}