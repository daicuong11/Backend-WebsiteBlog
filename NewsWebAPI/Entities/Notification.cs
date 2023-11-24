using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebAPI.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }


        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public int ArticleTargetID { get; set; }
        public int UserTargetID { get; set; }
        public int UserID { get; set; }
        public User UserCreate { get; set; }
    }
}
