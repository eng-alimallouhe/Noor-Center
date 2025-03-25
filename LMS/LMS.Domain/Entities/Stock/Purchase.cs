namespace LMS.Domain.Entities.Stock
{
    public class Purchase
    {
        // Primary key:
        public int PurchaseId { get; set; }

        //Foreign Key: SupplierId ==> one(Supplier)-to-many(Purchase) relationship
        public int SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public Supplier Supplier { get; set; } = new Supplier();
    }

}
