using LMS.Domain.Entities.Orders;
using LMS.Domain.Users;

namespace LMS.Domain.Entities.Stock
{
    public class Product
    {
        // Primary key:
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }

        //soft delete
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public ICollection<Discount> Discounts { get; set; } = [];
        public ICollection<InventoryLog> Logs { get; set; } = [];
        public ICollection<CartItem> CartItems { get; set; } = [];
    }
}
