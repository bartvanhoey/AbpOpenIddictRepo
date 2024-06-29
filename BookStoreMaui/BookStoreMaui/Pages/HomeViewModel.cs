using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.OpenIddict.Infra;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace BookStoreMaui.Pages;

public partial class HomeViewModel : ObservableObject
{
    private readonly IBookAppService _bookAppService;

    public HomeViewModel(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }
    
        
    
    
    
    public async Task OnAppearing()
    {
        var booksAsync = await _bookAppService.GetBooksAsync();
    }
}

public interface IBookAppService
{
    Task<IEnumerable<BookDto>> GetBooksAsync();
    // Task<Book> GetBookAsync(int id);
    // Task AddBookAsync(Book book);
    // Task UpdateBookAsync(Book book);
    // Task DeleteBookAsync(int id);
}

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

        var listAsync = await _httpService.GetListAsync($"{authorityUrl}/api/app/book", new GetBooksPagedRequestDto());

        return new List<BookDto>();
    }
}