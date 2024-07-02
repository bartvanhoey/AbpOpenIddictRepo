using BookStoreMaui.Models;
using BookStoreMaui.Services.Books;
using BookStoreMaui.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookStoreMaui.Pages.Books.Add;

public partial class AddBookViewModel(IBookService bookService, INavigationService navigate) : ObservableObject
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private DateTime _publishDate;
    [ObservableProperty] private float _price;
    
    
    [ObservableProperty]  BookType _selectedBookType = BookType.Undefined;

    public IReadOnlyList<string> BookTypes { get; } = Enum.GetNames(typeof(BookType));
    
    [RelayCommand]
    private async Task SaveBook()
    {
        await bookService.CreateBookAsync(new CreateBooDto(Name, SelectedBookType, PublishDate, Price));
        await navigate.ToBooksPage();
    }
}