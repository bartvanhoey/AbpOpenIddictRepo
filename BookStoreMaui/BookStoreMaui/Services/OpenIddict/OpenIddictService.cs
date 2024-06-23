using BookStoreMaui.Services.SecureStorage;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using static BookStoreMaui.Services.OpenIddict.OidcClientCreator;
using DisplayMode = IdentityModel.OidcClient.Browser.DisplayMode;

namespace BookStoreMaui.Services.OpenIddict
{
    public class OpenIddictService(IConfiguration configuration, ISecureStorageService storageService)
        : IOpenIddictService
    {
        public async Task<bool> AuthenticationSuccessful()
        {
            try
            {
                var oidcClient = CreateOidcClient(configuration);
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
            var oidcClient = CreateOidcClient(configuration);
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


//         private OidcClient CreateOidcClient()
//         {
//             var oIddict = configuration.GetSection(nameof(OpenIddictSettings)).Get<OpenIddictSettings>();
//             if (oIddict == null) throw new ArgumentNullException(nameof(OpenIddictSettings));
//
//             var oidcClientOptions = new OidcClientOptions
//             {
//                 Authority = oIddict.AuthorityUrl,
//                 ClientId = oIddict.ClientId,
//                 Scope = oIddict.Scope,
//                 RedirectUri = oIddict.RedirectUri,
//                 ClientSecret = oIddict.ClientSecret,
//                 PostLogoutRedirectUri = oIddict.PostLogoutRedirectUri,
//                 Browser = new WebAuthenticatorBrowser(),
//             };
//             var client = new OidcClient(oidcClientOptions);
//
// #if DEBUG
//             client.Options.HttpClientFactory = HttpClientResolver.GetHttpClientByPlatform;
// #endif
//             return client;
//         }
    }
}