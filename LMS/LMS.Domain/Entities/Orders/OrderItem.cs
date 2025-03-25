using LMS.Domain.Entities.Stock;

namespace LMS.Domain.Entities.Orders
{
    public class OrderItem
    {
        // Primary key:
        public int OrderItemId { get; set; }

        //Foreign Key: SellOrderId ==> one(SellOrder)-to-many(OrderItem) relationship
        public int SellOrderId { get; set; }

        //Foreign Key: ProductId ==> one(Product)-to-many(OrderItem) relationship
        public int ProductId { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal TotalPrice { get; set; }

        //Soft Delete:
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public SellOrder SellOrder { get; set; } = new SellOrder();
        public Product Product { get; set; } = new Product();
    }
}