using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsWebAPI.Entities
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }
        [Required(ErrorMessage = "Đường dẫn là trường bắt buộc.")]
        public string Path { get; set; }
        public string Desc { get; set; }
    }
}

