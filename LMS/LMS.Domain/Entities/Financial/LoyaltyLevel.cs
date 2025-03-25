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

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //navigation property:
        public ICollection<Customer> Customers { get; set; } = [];
    }
}
