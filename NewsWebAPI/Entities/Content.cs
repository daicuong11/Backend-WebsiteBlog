using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class Content
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentID { get; set; }
        public string? ContentTitle { get; set; }
        public string? ContentBody { get; set; }
        [NotMapped]
        public IFormFile ContentImage { get; set; }
        public string? ContentImagePath { get; set; }

        //khóa ngoại
        public int ArticleID { get; set; }
        [JsonIgnore]
        public Article? Article { get; set; }
    }
}
