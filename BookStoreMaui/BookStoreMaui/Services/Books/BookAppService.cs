using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.OpenIddict.Infra;
using FluentResults;
using Microsoft.Extensions.Configuration;

namespace BookStoreMaui.Services.Books;

public class BookAppService(
    IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto, Guid> httpService,
    IConfiguration config)
    : IBookAppService
{
    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        var result = await httpService.GetListAsync($"{config.GetAuthUrl()}/api/app/book", new GetBooksPagedRequestDto());
        return result.IsSuccess ? result.Value.Items : new List<BookDto>();
    }

    public async Task DeleteBookAsync(Guid bookDtoId) 
        => await httpService.DeleteAsync($"{config.GetAuthUrl()}/api/app/book", bookDtoId);

    public async Task<BookDto?> CreateBookAsync(CreateBooDto bookDto)
    {
        var result = await httpService.CreateAsync($"{config.GetAuthUrl()}/api/app/book", bookDto);
        return result.IsSuccess ? result.Value.Items.FirstOrDefault() : null;
                
    }

    public async Task<BookDto?> GetBookAsync(string bookId)
    {
        var result = await httpService.GetAsync($"{config.GetAuthUrl()}/api/app/book/{bookId}");
        return result.IsSuccess ? result.Value : null;
    }
}