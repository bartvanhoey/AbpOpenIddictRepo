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
            var oidcClient = configuration.GetOidcSettings().CreateClient();
            var login = await oidcClient.LoginAsync(new LoginRequest());
                
            if (login.IsNotAuthenticated()) return false;
                
            await storageService.SetOpenIddictTokensAsync(login);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    async Task IOpenIddictService.LogoutAsync()
    {
        var oidcClient = configuration.GetOidcSettings().CreateClient();
        try
        {
            var result = await oidcClient.LogoutAsync(new LogoutRequest
            {
                IdTokenHint = await storageService.GetIdentityTokenAsync(),
                BrowserDisplayMode = DisplayMode.Hidden,
            });

            if (result.IsError) await Task.CompletedTask;
            else
            {
                await storageService.RemoveOpenIddictTokensAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsUserLoggedInAsync()
    {
        var accessToken = await storageService.GetAccessTokenAsync();
        return AccessTokenValidator.IsAccessTokenValid(accessToken);
    }



}