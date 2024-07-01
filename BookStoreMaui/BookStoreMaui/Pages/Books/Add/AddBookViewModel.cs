using BookStoreMaui.Services.Books;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookStoreMaui.Pages.Books.Add;

public partial class AddBookViewModel(IBookAppService bookAppService) : ObservableObject
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private DateTime _publishDate;
    [ObservableProperty] private float _price;

    [RelayCommand]
    private async Task SaveBook()
    {
        await bookAppService.AddBookAsync(new CreateBooDto(Name, PublishDate, Price));
    }
}