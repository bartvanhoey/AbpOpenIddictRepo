using Microsoft.Extensions.Configuration;

namespace BookStoreMaui.Services.OpenIddict.Infra;

public static class ConfigurationExtensions
{
    public static OpenIddictSettings GetOidcSettings(this IConfiguration configuration)
    {
        var oIddict = configuration.GetSection(nameof(OpenIddictSettings)).Get<OpenIddictSettings>();
        if (oIddict == null) throw new ArgumentNullException(nameof(OpenIddictSettings));
        return oIddict;
    }
}