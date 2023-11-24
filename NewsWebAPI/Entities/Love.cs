using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class Love
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoveID { get; set; }

        public int UserTargetID { get; set; }
        public int ArticleID { get; set; }
        [JsonIgnore]
        public Article Article {  get; set; }
    }
}
