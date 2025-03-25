namespace LMS.Domain.Entities.Stock
{
    public class Book : Product
    {
        //Foreign Key: CategoryId ==> one(Genre)-to-many(Book) relationship
        public int GenreId { get; set; }

        //Foreign Key: AuthorId ==> one(Author)-to-many(Author) relationship
        public int AuthorId { get; set; }
        
        public string ISBN { get; set; } = string.Empty;
        public int Pages { get; set; }
        public decimal RentalCost { get; set; }
        public int PublishedYear { get; set; }

        //Navigation Property: many-to-many
        public ICollection<Publisher> Publishers { get; set; } = [];

        public Genre Genre { get; set; } = new Genre();
        public Author Author { get; set; } = new Author();
    }
}