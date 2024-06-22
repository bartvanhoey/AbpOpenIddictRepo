using IdentityModel.OidcClient;

namespace BookStoreMaui.Services.OpenIddict;

public static class OidcClientOptionsExtensions
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