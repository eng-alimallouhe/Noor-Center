using LMS.Domain.Entities.Users;
using LMS.Domain.Enums.Orders;

namespace LMS.Domain.Entities.Orders
{
    public class Order
    {
        //primary key:
        public int OrderId { get; set; }

        //Foreign Key: CustomerId ==> one(Customer)-to-many(Order) relationship
        public int CustomerId { get; set; }

        //Foreign Key: EmployeeId ==> one(Employee)-to-many(Order) relationship
        public int EmployeeId { get; set; }

        //Foreign Key: DepartmentId ==> one(Department)-to-many(Order) relationship
        public int DepartmentId { get; set; }

        public OrderStatus Status { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; } 
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Cost { get; set; }

        //soft delete:
        public bool IsActive { get; set; } = true;

        //timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //nabigation property:
        public Customer Customer { get; set; } = new Customer();
        public Employee Employee { get; set; } = new Employee();
        public Department Department { get; set; } = new Department();


    }
}
