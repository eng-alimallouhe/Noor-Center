namespace LMS.Domain.Entities.Users
{
    public class OtpCode
    {
        //primary Key:
        public int OtpCodeId { get; set; }
        
        //foreign Key: UserId ==> one(user) to one(OtpCode) relationship
        public int UserId { get; set; }
        
        public string OtpCodeValue { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public int FailedAttempts { get; set; }

        //timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddMinutes(10);
        
        //navigation property:
        public User User { get; set; } = new User();
    }
}
