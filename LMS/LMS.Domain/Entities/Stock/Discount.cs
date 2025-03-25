namespace LMS.Domain.Entities.Stock
{
    public class Discount
    {
        // Primary key:
        public int DiscountId { get; set; }

        // Foreign key:
        public int ProductId { get; set; }

        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public Product Product { get; set; } = new Product();
    }

}
