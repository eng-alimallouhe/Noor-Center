namespace LMS.Domain.Entities.Stock
{
    public class Author
    {
        // Primary key:
        public int AuthorId { get; set; }

        public string AuthorName { get; set; } = string.Empty;
        public string AuthorDescription { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public ICollection<Book> Books { get; set; } = [];
    }

}
