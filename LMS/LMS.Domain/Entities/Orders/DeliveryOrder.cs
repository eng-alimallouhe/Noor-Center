using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.Orders
{
    public class DeliveryOrder
    {
        //Foreign Key: AddressId ==> one(Address)-to-one(PrintOrder) relationship
        public int AddressId { get; set; }

        public string OrderName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
    }
}
