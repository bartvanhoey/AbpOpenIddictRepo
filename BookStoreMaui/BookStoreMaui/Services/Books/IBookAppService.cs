namespace BookStoreMaui.Services.Books;

public interface IBookAppService
{
    Task<IEnumerable<BookDto>> GetBooksAsync();
    // Task<Book> GetBookAsync(int id);
    // Task AddBookAsync(Book book);
    // Task UpdateBookAsync(Book book);
    // Task DeleteBookAsync(int id);
    Task DeleteBookAsync(Guid bookDtoId);
    Task<BookDto?> CreateBookAsync(CreateBooDto bookDto);
    Task<BookDto?> GetBookAsync(string bookId);
}