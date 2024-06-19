﻿using System.IdentityModel.Tokens.Jwt;
using BookStoreMaui.Models;
using BookStoreMaui.Services.SecureStorage;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using static System.String;
using DisplayMode = IdentityModel.OidcClient.Browser.DisplayMode;

namespace BookStoreMaui.Services.OpenIddict
{
    public class OpenIddictService : IOpenIddictService
    {
        private readonly IConfiguration _configuration;
        private readonly ISecureStorageService _storageService;

        public OpenIddictService(IConfiguration configuration, ISecureStorageService storageService)
        {
            _configuration = configuration;
            _storageService = storageService;
        }

        public async Task<bool> AuthenticationSuccessful()
        {
            var oidcClient = CreateOidcClient();
            var result = await oidcClient.LoginAsync(new LoginRequest());

            var isAuthenticated = !IsNullOrWhiteSpace(result.AccessToken) &&
                                  !IsNullOrWhiteSpace(result.IdentityToken) &&
                                  !IsNullOrWhiteSpace(result.RefreshToken);

            if (!isAuthenticated) return false;

            await _storageService.SetAccessTokenAsync(result.AccessToken);
            await _storageService.SetRefreshTokenAsync(result.RefreshToken);
            await _storageService.SetIdentityTokenTokensAsync(result.IdentityToken);

            return true;
        }

        async Task IOpenIddictService.LogoutAsync()
        {
            var oidcClient = CreateOidcClient();
            try
            {
                var result = await oidcClient.LogoutAsync(new LogoutRequest
                {
                    IdTokenHint = await _storageService.GetIdentityTokenTokenAsync(),
                    BrowserDisplayMode = DisplayMode.Hidden,
                });

                if (result.IsError) await Task.CompletedTask;
                else
                {
                    await _storageService.RemoveAccessTokenAsync();
                    await _storageService.RemoveRefreshTokenAsync();
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
            var accessToken = await _storageService.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(accessToken))
                return
                    false;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var isValid = token != null && token.ValidFrom < DateTime.UtcNow && token.ValidTo > DateTime.UtcNow;
            return isValid;
        }


        private OidcClient CreateOidcClient()
        {
            var oIddict = _configuration.GetSection(nameof(OpenIddictSettings)).Get<OpenIddictSettings>();
            var options = new OidcClientOptions
            {
                Authority = oIddict.AuthorityUrl,
                ClientId = oIddict.ClientId,
                Scope = oIddict.Scope,
                RedirectUri = oIddict.RedirectUri,
                ClientSecret = oIddict.ClientSecret,
                PostLogoutRedirectUri = oIddict.PostLogoutRedirectUri,
                Browser = new WebAuthenticatorBrowser()
            };

            return new OidcClient(options);
        }
    }
}