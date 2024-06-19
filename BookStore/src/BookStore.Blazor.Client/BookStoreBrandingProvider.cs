using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace BookStore.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class BookStoreBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BookStore";
}
