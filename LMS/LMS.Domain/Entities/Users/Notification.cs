namespace LMS.Domain.Entities.Users
{
    public class Notification
    {
        //primary key: 
        public int NotificationId { get; set; }

        // Foreign Key: UserId ==> one(user) to many(notifications) relationship
        public int UserId { get; set; }

        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false; 
        public DateTime? ReadAt { get; set; }
        public string RedirectUrl { get; set; } = string.Empty;

        // Navigation Property:
        public User User { get; set; } = new User();
    }
}
