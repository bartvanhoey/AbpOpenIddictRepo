using BookStoreMaui.Services.Books.Dtos;
using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.OpenIddict.Infra;
using Microsoft.Extensions.Configuration;

namespace BookStoreMaui.Services.Books;

public class BookService(
    IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto, Guid> httpService,
    IConfiguration config)
    : IBookService
{
    public async Task<IEnumerable<BookDto>> GetBooksAsync() 
        => (await httpService.GetListAsync($"{config.GetBookApiUrl()}", new GetBooksPagedRequestDto())).Items;

    public async Task<BookDto?> UpdateBookAsync(UpdateBookDto book) 
        => (await httpService.UpdateAsync($"{config.GetBookApiUrl()}/{book.Id}", book)).Items.FirstOrDefault();

    public async Task DeleteBookAsync(Guid bookDtoId) 
        => await httpService.DeleteAsync($"{config.GetBookApiUrl()}", bookDtoId);

    public async Task<BookDto?> CreateBookAsync(CreateBooDto bookDto) 
        => (await httpService.CreateAsync($"{config.GetBookApiUrl()}", bookDto)).Items.FirstOrDefault();

    public async Task<BookDto?> GetBookAsync(string bookId) 
        => await httpService.GetAsync($"{config.GetBookApiUrl()}/{bookId}");
}