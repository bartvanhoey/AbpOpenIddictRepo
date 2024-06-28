using BookStoreMaui.Functional;
using BookStoreMaui.Services.SecureStorage;
using FluentResults;

namespace BookStoreMaui.Services.Http;

public class HttpService<T, TC, TU, TG> : HttpServiceBase<T, TC, TU, TG>, IHttpService<T, TC, TU, TG>
    where T : class
    where TC : class
    where TU : class
    where TG : class
{
    public HttpService(ISecureStorageService secureStorageService) : base(secureStorageService)
    {
    }

    public async Task<Result<ListResult<T>>> GetListAsync(string uri, TG? getListInputDto = default)
    {
        if (getListInputDto == null) return new Result<ListResult<T>>();

        var httpResponse = await (await GetHttpClientAsync()).Value.GetAsync(ComposeUri(uri, getListInputDto));

        var json = await httpResponse.Content.ReadAsStringAsync();
        if (json == "[]" || json.IsNullOrWhiteSpace()) return Result.Ok(new ListResult<T>());

        var listResult = new ListResult<T>(json.ToType<List<T>>());
        return Result.Ok(listResult);
    }

    public Task<Result<ListResult<T>>> UpdateAsync(string uri, TU updateInputDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ListResult<T>>> CreateAsync(string url, TC createInputDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> GetAsync(string uri)
    {
        throw new NotImplementedException();
    }
}