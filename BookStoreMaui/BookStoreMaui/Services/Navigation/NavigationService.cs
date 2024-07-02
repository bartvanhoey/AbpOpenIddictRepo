using BookStoreMaui.Pages.Books;
using BookStoreMaui.Pages.Books.Add;
using BookStoreMaui.Pages.Books.Edit;

namespace BookStoreMaui.Services.Navigation;

public class NavigationService(IServiceProvider services) : INavigationService
{
    private INavigation Navigation => Application.Current?.MainPage?.Navigation ?? throw new Exception();

    public async Task ToAddBookPage() => await NavigateToPage<AddBookPage>();
    public async Task ToBooksPage() => await NavigateToPage<BooksPage>();

    private Task NavigateToPage<T>() where T : Page
    {
        var page = ResolvePage<T>();
        if(page is not null)
            return Navigation.PushAsync(page, true);
        throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
    }
    private T? ResolvePage<T>() where T : Page
        => services.GetService<T>();


    public Task NavigateBack() =>
        Navigation.NavigationStack.Count > 1
            ? Navigation.PopAsync()
            : throw new InvalidOperationException("No pages to navigate back to!");

}