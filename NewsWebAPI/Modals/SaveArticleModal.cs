using NewsWebAPI.Entities;

namespace NewsWebAPI.Modals
{
    public class SaveArticleModal
    {
        public int SavedArticleID { get; set; }

        public DateTime? SavedAt { get; set; }

        public int UserTargetID { get; set; }
        public int ArticleID { get; set; }
    }
}
