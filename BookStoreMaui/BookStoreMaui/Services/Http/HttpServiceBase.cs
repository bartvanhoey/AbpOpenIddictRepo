using BookStoreMaui.Services.SecureStorage;
using IdentityModel.Client;

namespace BookStoreMaui.Services.Http;

public class HttpServiceBase<T, TC, TU, TG>
{
    public HttpServiceBase(ISecureStorageService secureStorageService)
    {
        StorageService = secureStorageService;
    }   

    private ISecureStorageService StorageService { get; set; }

    protected async Task<Lazy<HttpClient>> GetHttpClientAsync()
    {
        var httpClient = new Lazy<HttpClient>(() => new HttpClient());
        var accessToken = await StorageService.GetAccessTokenAsync();
        httpClient.Value.SetBearerToken(accessToken);
        return httpClient;
    }

    protected static string ComposeUri(string uri, TG getListInputDto)
    {
        if (getListInputDto is IPagedListDto pagedListDto)
            return uri.Contains("?")
                ? $"{uri}&skipCount={pagedListDto.SkipCount}&maxResultCount={pagedListDto.MaxResultCount}"
                : $"{uri}?skipCount={pagedListDto.SkipCount}&maxResultCount={pagedListDto.MaxResultCount}";
        return uri;
    }
    
    
}