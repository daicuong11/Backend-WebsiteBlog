using NewsWebAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebAPI.Modals
{
    public class ContentModal
    {
        public int ContentID { get; set; }
        public string? ContentTitle { get; set; }
        public string? ContentBody { get; set; }
        [NotMapped]
        public IFormFile? ContentImage { get; set; }
        public string? ContentImagePath { get; set; }

        public int ContentIndex { get; set; }

        //khóa ngoại
        public int ArticleID { get; set; }
    }
}
