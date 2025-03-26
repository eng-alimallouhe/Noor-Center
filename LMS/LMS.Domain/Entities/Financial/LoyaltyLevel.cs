using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.Financial
{
    public class LoyaltyLevel
    {
        //Primary Key:
        public int LevelId { get; set; }

        public string LevelName { get; set; } = string.Empty;
        public int RequiredPoints { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string LevelDescription { get; set; } = string.Empty;

        //soft delete: 
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //navigation property:
        public ICollection<Customer> Customers { get; set; } = [];
    }
}
