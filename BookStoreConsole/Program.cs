using BookStoreConsole.Services.Authors;
using BookStoreConsole.Services.Authors.Dtos;
using BookStoreConsole.Services.Books;
using BookStoreConsole.Services.Books.Dtos;
using BookStoreConsole.Services.Http;
using BookStoreConsole.Services.SecureStorage;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTransient<ISecureStorageService, SecureStorageService>();

// Books
const string bookApiUrl = "https://localhost:44336/api/app/book";
services.AddTransient<IHttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>,
    HttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>>();

services.AddTransient<IBookService, BookService>(options
    => new BookService(
        options
            .GetRequiredService<IHttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>>(),
        bookApiUrl));

var bookService = services.BuildServiceProvider().GetRequiredService<IBookService>();
var getBooks = await bookService.GetBooksAsync();
var createdBook = await bookService.CreateBookAsync(new CreateBookDto("Book 5", BookType.Adventure, DateTime.Now, 10.0f));
var updatedBook = await bookService.UpdateBookAsync(new UpdateBookDto(createdBook!.Id, "Book 5 Updated",
    BookType.ScienceFiction, DateTime.Now.AddMonths(5), 10.0f));
var getBook = await bookService.GetBookAsync(createdBook.Id.ToString());
await bookService.DeleteBookAsync(createdBook.Id);

// Authors
const string authorApiUrl = "https://localhost:44336/api/app/author";
services.AddTransient<IHttpService<AuthorDto, CreateAuthorDto, UpdateAuthorDto, GetAuthorsPagedRequestDto, Guid>,
    HttpService<AuthorDto, CreateAuthorDto, UpdateAuthorDto, GetAuthorsPagedRequestDto, Guid>>();

services.AddTransient<IAuthorService, AuthorService>(options
    => new AuthorService(
        options
            .GetRequiredService<
                IHttpService<AuthorDto, CreateAuthorDto, UpdateAuthorDto, GetAuthorsPagedRequestDto, Guid>>(),
        authorApiUrl));

var authorService = services.BuildServiceProvider().GetRequiredService<IAuthorService>();
var getAuthors1 = await authorService.GetAuthorsAsync();
var createdAuthor =
    await authorService.CreateAuthorAsync(new CreateAuthorDto("Author 5", DateTime.Now.AddYears(-50), "Short Bio"));
var updatedAuthor = await authorService.UpdateAuthorAsync(new UpdateAuthorDto(createdAuthor!.Id, "Author 5 Updated",
     DateTime.Now.AddYears(-5), "ShortBio Updated"));
var getAuthor = await authorService.GetAuthorAsync(createdAuthor.Id.ToString());
var getAuthors2 = await authorService.GetAuthorsAsync();
await authorService.DeleteAuthorAsync(createdAuthor.Id);

Console.ReadLine();