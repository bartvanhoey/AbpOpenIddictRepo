using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.OpenIddict.Infra;
using Microsoft.Extensions.Configuration;

namespace BookStoreMaui.Services.Books;

public class BookAppService : IBookAppService
{
    private readonly IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto, Guid> _httpService;
    private readonly IConfiguration _config;

    public BookAppService(IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto, Guid> httpService, IConfiguration config)
    {
        _httpService = httpService;
        _config = config;
    }
    
    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        var result = await _httpService.GetListAsync($"{_config.GetAuthUrl()}/api/app/book", new GetBooksPagedRequestDto());
        return result.IsSuccess ? result.Value.Items : new List<BookDto>();
    }

    public async Task DeleteBookAsync(Guid bookDtoId)
    {
        await _httpService.DeleteAsync($"{_config.GetAuthUrl()}/api/app/book", bookDtoId);
        // return result.IsSuccess ? result.Value.Items : new List<BookDto>();
    }
}