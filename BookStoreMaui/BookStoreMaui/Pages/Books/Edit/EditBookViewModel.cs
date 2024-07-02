using BookStoreMaui.Models;
using BookStoreMaui.Services.Books;
using BookStoreMaui.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookStoreMaui.Pages.Books.Edit;

[QueryProperty(nameof(BookId), nameof(BookId))]
public partial class EditBookViewModel(IBookAppService bookAppService, INavigationService navigate) : ObservableObject
{
    [ObservableProperty] private string? _bookId;

    [ObservableProperty] private BookDto? _book;

    public IReadOnlyList<string> BookTypes { get; } = Enum.GetNames(typeof(BookType));

    public async Task OnAppearing()
    {
        if (BookId != null) Book = await bookAppService.GetBookAsync(BookId);
    }

    [RelayCommand]
    private async Task SaveBook()
    {
        if (Book == null) return;
        await bookAppService.UpdateBookAsync(new UpdateBookDto(Book.Id, Book.Type, Book.Price, Book.PublishDate,
            Book.Name));
        await navigate.ToBooksPage();
    }
}