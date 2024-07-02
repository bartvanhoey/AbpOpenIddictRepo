using System.Reflection;
using BookStoreMaui.Models;
using BookStoreMaui.Pages;
using BookStoreMaui.Pages.Books;
using BookStoreMaui.Pages.Books.Add;
using BookStoreMaui.Pages.Books.Edit;
using BookStoreMaui.Pages.Home;
using BookStoreMaui.Services.Books;
using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.Http.Infra;
using BookStoreMaui.Services.Navigation;
using BookStoreMaui.Services.OpenIddict;
using BookStoreMaui.Services.OpenIddict.Infra;
using BookStoreMaui.Services.SecureStorage;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace BookStoreMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();
        
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
             .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-solid-900.ttf", "FASolid");
                fonts.AddFont("fa-regular-400.ttf", "FARegular");
                fonts.AddFont("fa-brands-400.ttf", "FABrands");
            });
    
        // Add the appsettings.json file to the configuration
        var assembly = typeof(App).GetTypeInfo().Assembly;
        
        
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false,false);

        builder.Services.AddTransient<WebAuthenticatorBrowser>();
        
        builder.Services.AddTransient<ISecureStorageService, SecureStorageService>();
        builder.Services.AddTransient<IOpenIddictService, OpenIddictService>();
        
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();
        
        builder.Services.AddTransient<LogoutPage>();
        builder.Services.AddTransient<LogoutViewModel>();
        
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<HomeViewModel>();
        
        builder.Services.AddTransient<BooksPage>();
        builder.Services.AddTransient<BooksViewModel>();
        
        builder.Services.AddTransient<AddBookPage>();
        builder.Services.AddTransient<AddBookViewModel>();
        
        builder.Services.AddTransient<EditBookPage>();
        builder.Services.AddTransient<EditBookViewModel>();

        builder.Services.AddTransient<IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>, HttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto, Guid>>();
        
        builder.Services.AddTransient<IBookAppService, BookAppService>();
        
        builder.Services.AddSingleton<INavigationService, NavigationService>();


#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
        
        
    }
    
    
}

public class GetBooksPagedRequestDto : PagedRequestDto
{
}

public class UpdateBookDto
{
}

public  class CreateBooDto
{
    public CreateBooDto(string? name, BookType bookType, DateTime publishDate, float price)
    {
        Name = name;
        PublishDate = publishDate;
        Type = bookType;
        Price = price;
    }

    public BookType Type { get; set; }
    
    public float Price { get; set; }

    public DateTime PublishDate { get; set; }

    public string? Name { get; set; }
}

public class BookDto
{
    public BookDto()
    {
    }

    public BookDto(string? name, DateTime publishDate, float price)
    {
        Name = name;
        PublishDate = publishDate;
        Price = price;
    }

    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate{ get; set;  }

    public float Price { get; set; }
}

