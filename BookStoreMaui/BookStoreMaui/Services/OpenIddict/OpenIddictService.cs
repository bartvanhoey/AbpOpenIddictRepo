using System.IdentityModel.Tokens.Jwt;
using BookStoreMaui.Services.SecureStorage;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using static BookStoreMaui.Services.OpenIddict.HttpMessageHandlerResolver;
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
                var oidcClient = CreateOidcClient();
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
            var oidcClient = CreateOidcClient();
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

        public async Task<bool> IsUserLoggedInAsync() => await IsAccessTokenValidAsync();

        private async Task<bool> IsAccessTokenValidAsync()
        {
            var accessToken = await storageService.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(accessToken))
                return false;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var isValid = token != null && token.ValidFrom < DateTime.UtcNow && token.ValidTo > DateTime.UtcNow;
            return isValid;
        }


        private OidcClient CreateOidcClient()
        {
            var oIddict = configuration.GetSection(nameof(OpenIddictSettings)).Get<OpenIddictSettings>();
            if (oIddict == null) throw new ArgumentNullException(nameof(OpenIddictSettings));

            var oidcClientOptions = new OidcClientOptions
            {
                Authority = oIddict.AuthorityUrl,
                ClientId = oIddict.ClientId,
                Scope = oIddict.Scope,
                RedirectUri = oIddict.RedirectUri,
                ClientSecret = oIddict.ClientSecret,
                PostLogoutRedirectUri = oIddict.PostLogoutRedirectUri,
                Browser = new WebAuthenticatorBrowser(),
            };
            var client = new OidcClient(oidcClientOptions);

#if DEBUG
            client.Options.HttpClientFactory = OidcClientOptionsExtensions.GetHttpClientByPlatform;
#endif
            return client;
        }
    }
}