using System.Collections.ObjectModel;
using BookStoreMaui.Services.Books;
using BookStoreMaui.Services.Http;
using BookStoreMaui.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookStoreMaui.Pages.Books;

public partial class BooksViewModel : ObservableObject
{
    private readonly IBookAppService _bookAppService;

    public ObservableRangeCollection<BookDto> SourceItemDtos { get; set; } = new();
    
    public BooksViewModel(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }
    
        
    
    public async Task OnAppearing() 
        => SourceItemDtos.AddRange(await _bookAppService.GetBooksAsync());
    
    [RelayCommand]
    private void DeleteBook(BookDto bookDto)
    {
        _bookAppService.DeleteBookAsync(bookDto.Id);
    }
}