namespace LMS.Domain.Entities.Users
{
    public class Role
    {
        //primary key
        public int RoleId { get; set; }

        public string RoleType { get; set; } = string.Empty;

        //Soft Delete:
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
