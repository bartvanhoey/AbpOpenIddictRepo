namespace BookStoreWebApi.Dtos.Books
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate{ get; set;  }

        public float Price { get; set; }
        public Guid AuthorId { get; set; }
    }

    public enum BookType
    {
        Undefined,
        Adventure,
        Biography,
        Dystopia,
        Fantastic,
        Horror,
        Science,
        ScienceFiction,
        Poetry
    }
}