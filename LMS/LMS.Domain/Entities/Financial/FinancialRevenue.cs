﻿using LMS.Domain.Entities.Users;
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
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //Navigation Property: 
        public Customer Customer { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}
