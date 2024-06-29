using BookStoreMaui.Services.Http;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookStoreMaui.Pages.Books;

public partial class BooksViewModel : ObservableObject
{
    private readonly IBookAppService _bookAppService;

    
    
    public BooksViewModel(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }
    
        
    
    public void OnAppearing()
    {
        var booksAsync = _bookAppService.GetBooksAsync();
    }
}