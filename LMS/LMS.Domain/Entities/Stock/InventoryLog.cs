using LMS.Domain.Enums.Stock;

namespace LMS.Domain.Entities.Stock
{
    public class InventoryLog
    {
        // Primary key:
        public int InventoryLogId { get; set; }

        // Foreign key:
        public int ProductId { get; set; }

        public DateTime LogDate { get; set; }

        public LogType ChangeType { get; set; }
        public int ChangedQuantity { get; set; }
    }

}
