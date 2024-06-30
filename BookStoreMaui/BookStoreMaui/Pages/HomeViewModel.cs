using BookStoreMaui.Pages.Books;
using CommunityToolkit.Mvvm.ComponentModel;

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