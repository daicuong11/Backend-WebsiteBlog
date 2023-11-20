using System.ComponentModel.DataAnnotations;

namespace NewsWebAPI.Modals
{
    public class ArticleStatusModal
    {
        [Range(1, 5, ErrorMessage = "StatusCode must be between 1 and 5")]
        public int StatusCode { get; set; }
    }
}
