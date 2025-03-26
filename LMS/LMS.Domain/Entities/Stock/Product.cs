using LMS.Domain.Entities.Orders;
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
        public string ImgUrl { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public ICollection<Discount> Discounts { get; set; } = [];
        public ICollection<InventoryLog> Logs { get; set; } = [];
        public ICollection<CartItem> CartItems { get; set; } = [];
    }
}
