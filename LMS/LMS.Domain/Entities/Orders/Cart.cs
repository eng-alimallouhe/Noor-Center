using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.Orders
{
    public class Cart
    {
        // Primary key:
        public int CartId { get; set; }

        //Foreign Key: CustomerId ==> one(Cart)-to-one(Customer) relationship
        public int CustomerId { get; set; }

        public bool IsCheckedOut { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public Customer Customer { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = [];
    }
}
