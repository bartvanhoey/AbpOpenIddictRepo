using System.IdentityModel.Tokens.Jwt;
using IdentityModel.OidcClient;
using static System.String;

namespace BookStoreMaui.Services.OpenIddict;

public static class LoginResultExtensions
{
    public static bool IsNotAuthenticated(this LoginResult loginResult)
    {
        if (IsNullOrWhiteSpace(loginResult.AccessToken)) return true;
        if (IsNullOrWhiteSpace(loginResult.IdentityToken)) return true;
        if (IsNullOrWhiteSpace(loginResult.RefreshToken)) return true;

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(loginResult.AccessToken);
        var isValid = token != null && token.ValidFrom < DateTime.UtcNow && token.ValidTo > DateTime.UtcNow;
        return !isValid;
    }
}