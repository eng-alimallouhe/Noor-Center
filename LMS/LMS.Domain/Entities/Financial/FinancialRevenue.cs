using LMS.Domain.Enums.Finacial;
using LMS.Domain.Enums.Orders;

namespace LMS.Domain.Entities.Financial
{
    public class FinancialRevenue
    {
        //Primary Key:
        public int FinancialRevenueId { get; set; }

        //Foreign Key: CustomerId ==> one(Customer)-to-many(Payment) relationship
        public int CustomerId { get; set; }

        //Foreign Key: EmployeeId ==> one(Employee)-to-many(PrintOrder) relationship
        public int EmployeeId { get; set; }

        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Service Service { get; set; }
        
        //Soft Delete:
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
