using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreMaui.Pages.Books;

public partial class BooksPage : ContentPage
{
    private readonly BooksViewModel _vm;

    public BooksPage(BooksViewModel booksViewModel)
    {
        InitializeComponent();
        BindingContext = _vm = booksViewModel;
    }
    
    protected override void OnAppearing()
    {
     
        _vm.OnAppearing();
    }
    
}