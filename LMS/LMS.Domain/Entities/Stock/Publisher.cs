namespace LMS.Domain.Entities.Stock
{
    public class Publisher
    {
        // Primary key:
        public int PublisherId { get; set; }

        public string PublisherName { get; set; } = string.Empty;
        public string PublisherDescription { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public ICollection<Book> Books { get; set; } = [];
    }

}
