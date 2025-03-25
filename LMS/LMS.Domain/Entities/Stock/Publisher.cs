namespace LMS.Domain.Entities.Stock
{
    public class Publisher
    {
        // Primary key:
        public int PublisherId { get; set; }

        public string PublisherName { get; set; } = string.Empty;
        public string PublisherDescription { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public ICollection<Book> Books { get; set; } = [];
    }

}
