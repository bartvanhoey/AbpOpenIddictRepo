using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.OpenIddict.Infra;
using Microsoft.Extensions.Configuration;

namespace BookStoreMaui.Pages.Books;

public class BookAppService : IBookAppService
{
    private readonly IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto> _httpService;
    private readonly IConfiguration _configuration;

    public BookAppService(IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto> httpService, IConfiguration configuration)
    {
        _httpService = httpService;
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        var authorityUrl = _configuration.GetOidcSettings().AuthorityUrl;
        var result = await _httpService.GetListAsync($"{authorityUrl}/api/app/book", new GetBooksPagedRequestDto());
        return result.IsSuccess ? result.Value.Items : new List<BookDto>();
    }
}