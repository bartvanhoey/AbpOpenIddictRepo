using System.IdentityModel.Tokens.Jwt;
using IdentityModel.OidcClient;
using static System.String;
using static BookStoreMaui.Services.OpenIddict.AccessTokenValidator;

namespace BookStoreMaui.Services.OpenIddict;

public static class LoginResultExtensions
{
    public static bool IsNotAuthenticated(this LoginResult loginResult)
    {
        if (IsNullOrWhiteSpace(loginResult.AccessToken)) return true;
        if (IsNullOrWhiteSpace(loginResult.IdentityToken)) return true;
        if (IsNullOrWhiteSpace(loginResult.RefreshToken)) return true;
        return IsAccessTokenNotValid(loginResult.AccessToken);
    }
}

public static class AccessTokenValidator
{
    public static bool IsAccessTokenNotValid(string accessToken)
        => !IsAccessTokenValid(accessToken);
    public static bool IsAccessTokenValid(string accessToken)
    {
        if (IsNullOrWhiteSpace(accessToken)) return false;
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);
        var isValid = token != null && token.ValidFrom < DateTime.UtcNow && token.ValidTo > DateTime.UtcNow;
        return isValid;
    }
}


