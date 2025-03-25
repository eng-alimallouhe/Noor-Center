namespace LMS.Domain.Entities.Orders
{
    public class PrintOrder : Order
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int CopiesCount { get; set; } = 1;
        public decimal CopyCost { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
