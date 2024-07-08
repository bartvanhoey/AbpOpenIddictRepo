// See https://aka.ms/new-console-template for more information

using BookStoreConsole.Services.Books;
using BookStoreConsole.Services.Books.Dtos;
using BookStoreConsole.Services.Http;
using BookStoreConsole.Services.SecureStorage;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddTransient<IHttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>, 
            HttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>>();


services.AddTransient<ISecureStorageService, SecureStorageService>();

services.AddTransient<IBookService, BookService>(options => new BookService(
    options.GetRequiredService<IHttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>>(),
    "https://localhost:44336/api/app/book"));

var bookService = services.BuildServiceProvider().GetRequiredService<IBookService>();

var getBooks = await bookService.GetBooksAsync();

var createdBook = await bookService.CreateBookAsync(new CreateBookDto("Book 5", BookType.Adventure, DateTime.Now, 10.0f));

var updatedBook = bookService.UpdateBookAsync(new UpdateBookDto(createdBook!.Id, "Book 5 Updated", BookType.ScienceFiction, DateTime.Now.AddMonths(5), 10.0f));

var getBook = await bookService.GetBookAsync(createdBook.Id.ToString());

await bookService.DeleteBookAsync(createdBook.Id);




