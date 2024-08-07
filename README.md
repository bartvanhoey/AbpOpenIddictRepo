## Consume an ABP API (OpenIddict) from a .NET MAUI app

## Introduction

In a previous article, I demonstrated how to consume an ABP Framework API with a .NET MAUI app by username and password in a .NET MAUI page.

In this article, I will show you a more **secure way of authentication** where we make use of a web browser in the app that **redirects us to the ABP Framework login page**.

From version 6.0.0 the ABP Framework start to use **OpenIddict** instead of **IdentityServer** as the Identity Provider.

Keep in mind that I only include the most important code snippets in this article, but you can find the rest of the code needed in the GitHub repository.

### Source code

The source of the article is [available on GitHub](https://github.com/bartvanhoey/AbpOpenIddictRepo), but keep in mind that the source code is not production ready.

## Requirements

The following tools are needed to be able to run the solution and follow along.

- .NET 8.0 SDK
- vscode, Visual Studio 2022 or another compatible IDE
- ABP CLI 8.0.0
- Ngrok

## ABP Framework application

### Create a new ABP Framework application

```bash
  abp new BookStore -u blazor -o BookStore --preview
```

### Appsettings.json file of the DbMigrator project

Add the section below in the OpenIddict:Applications appsettings.json file of the DbMigrator project

```bash
    "BookStore_Maui": {
        "ClientId": "BookStore_Maui",
        "ClientSecret": "1q2w3e*",
        "RootUrl": "bookstore://"
    }
```

### Update the OpenIddictDataSeedContributor class of the Domain project

Add a **MauiBookStore client** section in the **CreateApplicationAsync** method of the **OpenIddictDataSeedContributor** class.

```bash
private async Task CreateApplicationsAsync()
{
    // other clients here ...

    // MauiBookStore client
    commonScopes.Add("offline_access");

    var mauiClientId = configurationSection["BookStore_Maui:ClientId"];
    if (!mauiClientId.IsNullOrWhiteSpace())
    {
        var mauiRootUrl = configurationSection["BookStore_Maui:RootUrl"];
        await CreateApplicationAsync(
            name: mauiClientId,
            type: OpenIddictConstants.ClientTypes.Confidential,
            consentType: OpenIddictConstants.ConsentTypes.Implicit,
            scopes: mauiScopes,
            grantTypes:
            [
                OpenIddictConstants.GrantTypes.AuthorizationCode,
                OpenIddictConstants.GrantTypes.RefreshToken
            ],
            secret: configurationSection["BookStore_Maui:ClientSecret"],
            redirectUri: $"{mauiRootUrl}",
            postLogoutRedirectUri: $"{mauiRootUrl}",
            displayName: "MauiBookStore"
        );
    }
 }

```

### Apply Migrations and Run the Application

- Run the DbMigrator project to apply the settings.
- After, check the **OpenIddictApplications** table of the database to see if the **BookStore_Maui** client has been added.
- Run the `BookStore.HttpApi.Host` application to start the API.

## Ngrok to the rescue

When you run the **ABP Framework API** on your local computer, the API is reachable on [https://localhost:\<your-port-number\>/api/\<path\>](https://localhost:<your-port-number>/api/<path>).

Although you can test out the **API endpoints** on your local machine, it will throw an exception in a .NET MAUI app.

```bash
    System.Net.WebException: Failed to connect to localhost/127.0.0.1:44330 ---> Java.Net.ConnectException: Failed to connect to localhost/127.0.0.1:44330
```

.NET MAUI considers localhost as its own localhost address (mobile device or emulator).

To overcome this problem you can **make use of ngrok** to **mirror your localhost address to a publicly available url**.
Go to the [ngrok page](https://ngrok.com/), create an account, and download and install Ngrok.

Open a command prompt in the root of the ABP Framework project and enter the command below to start ngrok

```bash
    // change the <replace-me-with-the-abp-api-port> with the port where the Swagger page is running on
    ngrok.exe http -region https://localhost:<replace-with-the-abp-api-port-number>/
```

The **ABP Framework API** is now publicly available on [https://7750-2a02-810d-af40-269c-f4e5-5286-1435-a6b7.ngrok-free.app](https://7750-2a02-810d-af40-269c-f4e5-5286-1435-a6b7.ngrok-free.app)

![ngrok in action](images/ngrok_in_action.png)

### Copy the ngrok url

Copy the **lower forwarding url** as you will need it for use in the .NET MAUI app.

## .NET MAUI app

### Create a new .NET MAUI app

### Let's Install some NuGet packages (in terminal window or NuGet package manager)

```bash
    dotnet add package CommunityToolkit.Mvvm --version 8.2.2
    dotnet add package IdentityModel --version 7.0.0
    dotnet add package Microsoft.Extensions.Configuration.Binder --version 8.0.1
    dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.0
    dotnet add package IdentityModel.OidcClient --version 6.0.0
    dotnet add package Microsoft.Extensions.FileProviders.Embedded --version 8.0.6
    dotnet add package System.IdentityModel.Tokens.Jwt --version 7.6.2
```

### Add appsettings.json file with OpenIddictSettings section to the root of the MAUI app

```bash
    "OpenIddictSettings": {
        "AuthorityUrl": "<https:replace-me-with-the-correct-url.ngrok-free.app>",
        "ClientId" : "BookStore_Maui",
        "RedirectUri" : "bookstore://",
        "Scope" : "openid offline_access address email profile roles BookStore",
        "ClientSecret" : "1q2w3e*"
    }
```

### Make the appsettings.json file an EmbeddedResource in the properties of the file

```bash
    <ItemGroup>
        <None Remove="appsettings.json" />
        <EmbeddedResource Include="appsettings.json" />
    </ItemGroup>
```

### Add a StorageServiceBase class to the Services/SecureStorage folder

```csharp
using System.Globalization;
using static System.ArgumentNullException;
using DeviceType = Microsoft.Maui.Devices.DeviceType;

namespace BookStoreMaui.Services.SecureStorage;

public class SecureStorageServiceBase
{
    protected static async Task SaveString(string key, string value) => await SetAsync(key, value);
    protected static async Task<string> GetString(string key, string defaultValue) => await GetAsync(key, defaultValue);

    private static async Task<string> GetAsync(string key, string defaultValue)
    {
        try
        {
            if (DeviceInfo.DeviceType == DeviceType.Physical)
                defaultValue = await Microsoft.Maui.Storage.SecureStorage.GetAsync(key) ?? defaultValue;
            else
            {
                try
                {
                    var hasKey = Preferences.Default.ContainsKey(key);
                    if (hasKey)
                    {
                        var value = Preferences.Default.Get(key, defaultValue);
                        return value ?? defaultValue;
                    }
                }
                catch (Exception exception)
                {
                }
            }
        }
        catch (Exception exception)
        {
        }

        return defaultValue;
    }


    private static async Task SetAsync(string key, string value)
    {
        try
        {
            ThrowIfNull(key);
            ThrowIfNull(value);
            if (DeviceInfo.DeviceType == DeviceType.Physical)
                await Microsoft.Maui.Storage.SecureStorage.SetAsync(key, value);
            else
                Preferences.Default.Set(key, value);
        }
        catch (Exception exception)
        {
        }
    }

    public async Task ClearAsync(string key)
    {
        try
        {
            if (DeviceInfo.DeviceType == DeviceType.Physical)
                await Microsoft.Maui.Storage.SecureStorage.SetAsync(key, "");
            else
            {
                try
                {
                    var hasKey = Preferences.Default.ContainsKey(key);
                    if (hasKey)
                    {
                        Preferences.Default.Remove(key);
                    }
                }
                catch (Exception exception)
                {
                }
            }
        }
        catch (Exception exception)
        {
        }
    }
}

```

### Add a IStorageService interface to the Services/SecureStorage folder

```csharp
    public interface ISecureStorageService
    {
        Task SetAccessTokenAsync(string accessToken);
        Task SetRefreshTokenAsync(string refreshToken);
        Task SetIdentityTokenAsync(string identityToken);
        Task SetOidcTokensAsync(LoginResult loginResult);
        Task<string> GetAccessTokenAsync();
        Task<string> GetRefreshTokenAsync();
        Task<string> GetIdentityTokenAsync();
        Task RemoveAccessTokenAsync();
        Task RemoveRefreshTokenAsync();
        Task RemoveIdentityTokenAsync();
        Task RemoveOidcTokensAsync();
    }

```

### Add a StorageService class to the Services/SecureStorage folder

```csharp

 using IdentityModel.OidcClient;

namespace BookStoreMaui.Services.SecureStorage;

public class SecureStorageService : SecureStorageServiceBase, ISecureStorageService
{
    public async Task SetAccessTokenAsync(string accessToken)
        => await SaveString("accessToken", accessToken);

    public async Task SetRefreshTokenAsync(string refreshToken)
        => await SaveString("refreshToken", refreshToken);

    public async Task SetIdentityTokenAsync(string identityToken)
        => await SaveString("identityToken", identityToken);

    public async Task SetOidcTokensAsync(LoginResult loginResult)
    {
        await SetIdentityTokenAsync(loginResult.IdentityToken);
        await SetAccessTokenAsync(loginResult.AccessToken);
        await SetRefreshTokenAsync(loginResult.RefreshToken);
    }

    public async Task<string> GetAccessTokenAsync()
        => await GetString("accessToken", "");

    public async Task<string> GetRefreshTokenAsync()
        => await GetString("refreshToken", "");
    public async Task<string> GetIdentityTokenAsync() 
        => await GetString("identityToken", "");


    public async Task RemoveAccessTokenAsync()
        => await ClearAsync("accessToken");

    public async Task RemoveRefreshTokenAsync()
        => await ClearAsync("refreshToken");

    public async Task RemoveIdentityTokenAsync()
        => await ClearAsync("identityToken");

    public async Task RemoveOidcTokensAsync()
    {
        await RemoveIdentityTokenAsync();
        await RemoveAccessTokenAsync();
        await RemoveRefreshTokenAsync();
    }
}

```

### Add an OpenIddictSettings class to the Services/OpenIddict folder

```csharp
public class OpenIddictSettings
{
    public string? AuthorityUrl { get; set; }
    public string? ClientId { get; set; }
    public string? RedirectUri { get; set; }
    public string? Scope { get; set; }
    public string? ClientSecret { get; set; }
    public string? PostLogoutRedirectUri { get; set; }
}
```

### Add an IOpenIddictService interface to the Services/OpenIddict folder

```csharp
namespace BookStoreMaui.Services.OpenIddict;

public interface IOpenIddictService
{
    Task<bool> AuthenticationSuccessful();
    Task LogoutAsync();
    Task<bool> IsUserLoggedInAsync();
}
```

### Add an OpenIddictService class to the Services/OpenIddict folder

```csharp

using BookStoreMaui.Services.OpenIddict.Infra;
using BookStoreMaui.Services.SecureStorage;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;

using DisplayMode = IdentityModel.OidcClient.Browser.DisplayMode;

namespace BookStoreMaui.Services.OpenIddict;

public class OpenIddictService(IConfiguration configuration, ISecureStorageService storageService)
    : IOpenIddictService
{
    public async Task<bool> AuthenticationSuccessful()
    {
        try
        {
            var login = await configuration.GetOidcSettings().CreateClient().LoginAsync(new LoginRequest());
            if (login.IsNotAuthenticated()) return false;

            await storageService.SetOidcTokensAsync(login);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            var logout = await configuration.GetOidcSettings().CreateClient().LogoutAsync(new LogoutRequest
            {
                IdTokenHint = await storageService.GetIdentityTokenAsync(),
                BrowserDisplayMode = DisplayMode.Hidden,
            });

            if (logout.IsError) await Task.CompletedTask;
            else
            {
                await storageService.RemoveOidcTokensAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsUserLoggedInAsync()
        => AccessTokenValidator.IsAccessTokenValid(await storageService.GetAccessTokenAsync());
}
```

### Add a WebAuthenticatorBrowser class to the Services/OpenIddict/Infra folder

```csharp

using IdentityModel.OidcClient.Browser;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace BookStoreMaui.Services.OpenIddict
{
    internal class WebAuthenticatorBrowser(string? callbackUrl = null) : IBrowser
    {
        private readonly string _callbackUrl = callbackUrl ?? "";

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options,
            CancellationToken cancellationToken = default)
        {
            try
            {
                 var authenticatorOptions = new WebAuthenticatorOptions
                {
                    Url = new Uri(options.StartUrl),
                    CallbackUrl = new Uri(string.IsNullOrEmpty(_callbackUrl) ? options.EndUrl : _callbackUrl),
                    PrefersEphemeralWebBrowserSession = true
                };

                var authResult = await WebAuthenticator.Default.AuthenticateAsync(authenticatorOptions);
                
                var authorizeResponse = ToRawIdentityUrl(options.EndUrl, authResult);
                return new BrowserResult
                {
                    Response = authorizeResponse
                };
            }
            catch (TaskCanceledException ex)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                };
            }
            catch (Exception ex)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                };
            }
        }

        private static string ToRawIdentityUrl(string redirectUrl, WebAuthenticatorResult result)
        {
            var parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
            var values = string.Join("&", parameters);
            return $"{redirectUrl}#{values}";
        }
    }
}

```

### Add an OidcClientCreator class to the Services/OpenIddict/Infra folder

```csharp
    using IdentityModel.OidcClient;
    using Microsoft.Extensions.Configuration;
    using static System.ArgumentNullException;

    namespace BookStoreMaui.Services.OpenIddict.Infra;

    public static class OidcClientCreator
    {
        public static OidcClient CreateClient(this OpenIddictSettings settings)
        {
            ThrowIfNull(settings);

            var oidcClientOptions = new OidcClientOptions
            {
                Authority = settings.AuthorityUrl,
                ClientId = settings.ClientId,
                Scope = settings.Scope,
                RedirectUri = settings.RedirectUri,
                ClientSecret = settings.ClientSecret,
                PostLogoutRedirectUri = settings.PostLogoutRedirectUri,
                Browser = new WebAuthenticatorBrowser(),
            };
            var client = new OidcClient(oidcClientOptions);

    #if DEBUG
            client.Options.HttpClientFactory = HttpClientByPlatformResolver.GetHttpClientByPlatform;
    #endif
            return client;

        }
    }
```

### Add an HttpClientByPlatformResolver class to the Services/OpenIddict/Infra folder

```csharp
    using IdentityModel.OidcClient;

    namespace BookStoreMaui.Services.OpenIddict.Infra;

    public static class HttpClientByPlatformResolver
    {
        public static HttpClient GetHttpClientByPlatform(this OidcClientOptions options)
            => new(GetHttpMessageHandlerByPlatform());

        private static HttpMessageHandler GetHttpMessageHandlerByPlatform()
        {
    #if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (_, cert, _, errors) =>
            {
                if (cert is { Issuer: "CN=localhost" })
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
    #elif IOS
            var handler = new NSUrlSessionHandler
            {
                TrustOverrideForUrl = (_, url, _) => url.StartsWith("https://localhost") || url.Contains(".ngrok-free.app"),
            };
            return handler;
    #else
            throw new PlatformNotSupportedException("Only Android and iOS supported.");
    #endif
        }
    }
```

### Cop/paste infrastructure files into the Services/OpenIddict/Infra folder

Copy **ConfigurationExtensions.cs**, **LoginResultExtensions.cs** and **AccessTokenValidator.cs** from the GitHub source code into the **Services/OpenIddict/Infra** folder.

### Copy/paste the Views (+ code-behind pages) and ViewModels into the Pages folder

Copy the **LoginPage.xaml**, **HomePage.xaml** and **LogoutPage.xaml** files from the GitHub source code into the **Pages** folder.

Copy the **LoginViewModel.cs** and **LogoutViewModel.cs** files from the GitHub source code into the **Pages** folder.

### Replace the content of the AppShell page

```bash
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="BookStoreMaui.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:pages="clr-namespace:BookStoreMaui.Pages"
    Title="BookStoreMaui">

    <ShellContent ContentTemplate="{DataTemplate pages:LoginPage}" FlyoutItemIsVisible="False" Route="LoginPage" />
    <ShellContent Title="Home" ContentTemplate="{DataTemplate pages:HomePage}" Route="HomePage" />
    <ShellContent Title="Logout" ContentTemplate="{DataTemplate pages:LogoutPage}" FlyoutItemIsVisible="True" Route="LogoutPage" />

</Shell>

```

### Register the Pages and ViewModels in MauiProgram.cs

```csharp
// ... other code here

// Add the appsettings.json file to the configuration
var assembly = typeof(App).GetTypeInfo().Assembly;
builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false,false);

builder.Services.AddTransient<ISecureStorageService, SecureStorageService>();
builder.Services.AddTransient<IOpenIddictService, OpenIddictService>();

builder.Services.AddTransient<LoginPage>();
builder.Services.AddTransient<LoginViewModel>();

builder.Services.AddTransient<LogoutPage>();
builder.Services.AddTransient<LogoutViewModel>();

return  builder.Build();
```

### Create a CallBackUri const in App.xaml.cs

```csharp
    public const string CallbackUri = "bookstore://";
```

## Android Specific Configuration

### WebAuthenticationCallbackActivity in the root of the Android project

Add a WebAuthenticationCallbackActivity class in the root of the Android project.

```csharp
using Android.App;
using Android.Content.PM;
using BookStoreMaui;

namespace MauiBookStore
{
    [Activity(Name ="MauiBookStore.WebAuthenticationCallbackActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
        DataScheme = App.CallbackUri)]
    public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    {
    }
}
```

### Update the AndroidManifest.xml file

I you try running your app now, you will probably get the error below:

```bash
AndroidManifest.xml(19, 5): [AMM0000]
android:exported needs to be explicitly specified for element <activity#MauiBookStore.WebAuthenticationCallbackActivity>.

Apps targeting Android 12 and higher are required to specify an explicit value for `android:exported` when the corresponding component has an intent filter defined.

See https://developer.android.com/guide/topics/manifest/activity-element#exported for details.

```

To overcome this problem, update your **AndroidManifest.xml** file

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
  <application
    android:allowBackup="true"
    android:icon="@mipmap/appicon"
    android:roundIcon="@mipmap/appicon_round"
    android:supportsRtl="true">
    <activity
      android:exported="true"
      android:launchMode="singleTop"
      android:noHistory="true"
      android:name="MauiBookStore.WebAuthenticationCallbackActivity">
      <intent-filter>
        <action android:name="android.intent.action.VIEW" />
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        <data android:scheme="bookstore" />
      </intent-filter>
    </activity>
  </application>
  <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
  <uses-permission android:name="android.permission.INTERNET" />
  <queries>
    <intent>
      <action
        android:name="android.support.customtabs.action.CustomTabsService" />
    </intent>
  </queries>
</manifest>
```

## iOS Specific Configuration

### Update the Info.plist file

Add the code below to the **Info.plist** file of the iOS project.

```xml  
<key>CFBundleURLTypes</key>
    <array>
        <dict>
            <key>CFBundleURLName</key>
            <string>My BookStore App</string>
            <key>CFBundleURLSchemes</key>
            <array>
                <string>bookstoremaui</string></array>
            <key>CFBundleTypeRole</key>
                <string>Editor</string>
        </dict>
    </array>
```

## Start API and .NET MAUI app

Run the **HttpApi.Host** project and make sure **Ngrok** is running too.
Copy the **Ngrok url** and replace the **AuthorityUrl** in the **appsettings.json** file of the **.NET MAUI app**.

Start the **.NET Maui app** and click the **Login** button to display the ABP Framework login page.

Enter the standard credentials (user name:**admin** - password: **1q2w3E\***) and click Login.

You will be redirected to the HomePage of the app. Et voilà! As you can see, you received an access token
from the **ABP Framework API**. Now you can start consuming the API!

Get the [source code](https://github.com/bartvanhoey/AbpOpenIddictRepo) on GitHub.

Enjoy and have fun!
