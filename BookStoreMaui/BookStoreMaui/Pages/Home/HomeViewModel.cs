using BookStoreMaui.Pages.Books;
using BookStoreMaui.Services.Books;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookStoreMaui.Pages;

public partial class HomeViewModel : ObservableObject
{
    private readonly IBookService _bookService;

    public HomeViewModel(IBookService bookService)
    {
        _bookService = bookService;
    }
    
        
    
    
    
    public async Task OnAppearing()
    {
        var booksAsync = await _bookService.GetBooksAsync();
    }
}