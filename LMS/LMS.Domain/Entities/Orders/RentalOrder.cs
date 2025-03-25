using LMS.Domain.Entities.Stock;

namespace LMS.Domain.Entities.Orders
{
    public class RentalOrder
    {
        //Foreign Key: BookId ==> one(Book)-to-many(RentalOrder) relationship
        public int BookId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InitialCost { get; set; }
        public decimal LateCost { get; set; } = 0.0m;

        //navigation property: 
        public Book Book { get; set; } = new Book();
    }
}
