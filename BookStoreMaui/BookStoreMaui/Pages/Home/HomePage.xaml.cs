namespace BookStoreMaui.Pages;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _vm;

    public HomePage(HomeViewModel homeViewModel)
    {
        BindingContext = _vm = homeViewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        _vm.OnAppearing();
    }
}