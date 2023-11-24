namespace NewsWebAPI.Modals
{
    public class NotificationModal
    {
        public int NotificationID { get; set; }

        public string Title { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsRead { get; set; } = false;

        public int ArticleTargetID { get; set; }
        public int UserTargetID { get; set; }
        public int UserID { get; set; }
    }
}
