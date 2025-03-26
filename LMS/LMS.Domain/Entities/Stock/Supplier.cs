namespace LMS.Domain.Entities.Stock
{
    public class Supplier
    {
        // Primary key:
        public int SupplierId { get; set; }

        public string SupplierName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public ICollection<Purchase> Purchases { get; set; } = [];
    }
}
