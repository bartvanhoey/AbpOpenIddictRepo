using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using static System.ArgumentNullException;

namespace BookStoreMaui.Services.OpenIddict;

public static class OidcClientCreator
{
    public static OidcClient CreateOidcClient(OpenIddictSettings settings)
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
        client.Options.HttpClientFactory = HttpClientResolver.GetHttpClientByPlatform;
#endif
        return client;

    }

    public static OidcClient CreateOidcClient(IConfiguration configuration)
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
        client.Options.HttpClientFactory = HttpClientResolver.GetHttpClientByPlatform;
#endif
        return client;
    }
}