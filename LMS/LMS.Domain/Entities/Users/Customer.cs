using LMS.Domain.Entities.Financial;
using LMS.Domain.Entities.Orders;
using Microsoft.VisualBasic;

namespace LMS.Domain.Entities.Users
{
    public class Customer : User
    {
        //Foreign Key: LevelId ==> one(customer)-to-one(level) relationship
        public int LevelId { get; set; }

        public decimal points { get; set; }

        //navigation property:
        public LoyaltyLevel Level { get; set; } = new LoyaltyLevel();
        public ICollection<Address> Addresses { get; set; } = [];
        public Cart Cart { get; set; } = new Cart();
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<FinancialRevenue> FinancialRevenues { get; set; } = [];
    }
}
