using LMS.Domain.Users;

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
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public ICollection<Purchase> Purchases { get; set; } = [];
    }
}
