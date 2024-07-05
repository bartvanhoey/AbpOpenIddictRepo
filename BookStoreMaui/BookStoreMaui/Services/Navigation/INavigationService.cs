namespace BookStoreMaui.Services.Navigation;

public interface INavigationService
{
    Task NavigateBack();
    Task ToAddBookPage();
    Task ToBooksPage();
    Task ToAddAuthorPage();
}