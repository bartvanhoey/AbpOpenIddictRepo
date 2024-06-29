using System.Reflection;
using BookStoreMaui.Functional;
using BookStoreMaui.Pages;
using BookStoreMaui.Services.Http;
using BookStoreMaui.Services.Http.Infra;
using BookStoreMaui.Services.OpenIddict;
using BookStoreMaui.Services.OpenIddict.Infra;
using BookStoreMaui.Services.SecureStorage;
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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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

        builder.Services.AddTransient<IHttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto>, HttpService<BookDto, CreateBooDto, UpdateBookDto, GetBooksPagedRequestDto>>();
        
        builder.Services.AddTransient<IBookAppService, BookAppService>();

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
}

public class BookDto
{
    public string? Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate{ get; set;  }

    public float Price { get; set; }
}

public enum BookType
{
    Undefined,
    Adventure,
    Biography,
    Dystopia,
    Fantastic,
    Horror,
    Science,
    ScienceFiction,
    Poetry
}