using BookStoreMaui.Functional;
using BookStoreMaui.Services.Http.Infra;
using BookStoreMaui.Services.SecureStorage;
using FluentResults;

namespace BookStoreMaui.Services.Http;

public class HttpService<T, TC, TU, TL, TD> : HttpServiceBase<T, TC, TU, TL, TD>, IHttpService<T, TC, TU, TL, TD>
    where T : class
    where TC : class
    where TU : class
    where TL : class
{
    public HttpService(ISecureStorageService secureStorageService) : base(secureStorageService)
    {
    }

    public async Task<Result<ListResultDto<T>>> GetListAsync(string uri, TL? getListRequestDto = default)
    {
        if (getListRequestDto == null) return new Result<ListResultDto<T>>();

        var httpResponse = await (await GetHttpClientAsync()).Value.GetAsync(ComposeUri(uri, getListRequestDto));

        var json = await httpResponse.Content.ReadAsStringAsync();
        if (json == "[]" || json.IsNullOrWhiteSpace()) return Result.Ok(new ListResultDto<T>());
        
        if (getListRequestDto is IPagedRequestDto)
        {
            var pagedResultDto = json.ToType<PagedResultDto<T>>();
            return Result.Ok(new ListResultDto<T>(pagedResultDto.Items, pagedResultDto.TotalCount));
        }

        var listResultDto = new ListResultDto<T>(json.ToType<List<T>>());
        return Result.Ok(listResultDto);
    }

    public Task<Result<ListResultDto<T>>> UpdateAsync(string uri, TU updateInputDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ListResultDto<T>>> CreateAsync(string url, TC createInputDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<T>> GetAsync(string uri)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string uri, TD id)
    {
        var httpResponse = await (await GetHttpClientAsync()).Value.DeleteAsync($"{uri}/{id}" );
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Failed to delete");
        }
    }
}