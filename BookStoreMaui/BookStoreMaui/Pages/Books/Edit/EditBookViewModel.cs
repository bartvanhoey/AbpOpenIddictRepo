using BookStoreMaui.Models;
using BookStoreMaui.Services.Books;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookStoreMaui.Pages.Books.Edit;

[QueryProperty(nameof(BookId), nameof(BookId))]
public partial class EditBookViewModel(IBookAppService bookAppService) : ObservableObject
{
    [ObservableProperty]
    private string? _bookId;

    [ObservableProperty]
    private BookDto? _book;

    public IReadOnlyList<string> BookTypes { get; } = Enum.GetNames(typeof(BookType));
    
    public async Task OnAppearing()
    {
       Book = await bookAppService.GetBookAsync(BookId);
    }
    
    

    
}