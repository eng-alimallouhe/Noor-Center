namespace LMS.Domain.Entities.Stock
{
    public class Genre
    {
        // Primary key:
        public int GenreId { get; set; }

        public string GenreName { get; set; } = string.Empty;
        public string GenreDescription { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public ICollection<Book> Books { get; set; } = [];
    }
}
