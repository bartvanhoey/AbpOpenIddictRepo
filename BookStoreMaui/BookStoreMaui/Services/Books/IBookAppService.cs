﻿namespace BookStoreMaui.Services.Books;

public interface IBookAppService
{
    Task<IEnumerable<BookDto>> GetBooksAsync();
    Task<BookDto?> UpdateBookAsync(UpdateBookDto book);
    Task DeleteBookAsync(Guid bookDtoId);
    Task<BookDto?> CreateBookAsync(CreateBooDto bookDto);
    Task<BookDto?> GetBookAsync(string bookId);
}