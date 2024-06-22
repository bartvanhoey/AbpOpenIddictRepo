using IdentityModel.OidcClient.Browser;
using static System.String;
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
                var callbackUrl = IsNullOrEmpty(_callbackUrl) ? options.EndUrl : _callbackUrl;

                var authenticatorOptions = new WebAuthenticatorOptions
                {
                    Url = new Uri(options.StartUrl),
                    CallbackUrl = new Uri(callbackUrl),
                    PrefersEphemeralWebBrowserSession = true
                };;

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
            var values = Join("&", parameters);
            return $"{redirectUrl}#{values}";
        }
    }
}