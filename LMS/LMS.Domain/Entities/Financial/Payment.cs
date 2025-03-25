namespace LMS.Domain.Entities.Financial
{
    public class Payment
    {
        //primary key: 
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }
        public string Reasone { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
