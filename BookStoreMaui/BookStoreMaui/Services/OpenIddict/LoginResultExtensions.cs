using IdentityModel.OidcClient;
using static System.String;

namespace BookStoreMaui.Services.OpenIddict;

public  static class LoginResultExtensions
{
    public static bool IsNotAuthenticated(this LoginResult loginResult) =>
        IsNullOrWhiteSpace(loginResult.AccessToken) ||
        IsNullOrWhiteSpace(loginResult.IdentityToken) ||
        IsNullOrWhiteSpace(loginResult.RefreshToken);
}