using System.Text;
using BookStoreMaui.Functional;
using BookStoreMaui.Services.Http.Infra;
using BookStoreMaui.Services.SecureStorage;
using FluentResults;
using static FluentResults.Result;

namespace BookStoreMaui.Services.Http;

public class HttpService<T, TC, TU, TL, TD>(ISecureStorageService secureStorageService)
    : HttpServiceBase<T, TC, TU, TL, TD>(secureStorageService), IHttpService<T, TC, TU, TL, TD>
    where T : class
    where TC : class
    where TU : class
    where TL : class
{
    public async Task<Result<ListResultDto<T>>> GetListAsync(string uri, TL? getListRequestDto = default)
    {
        if (getListRequestDto == null) return new Result<ListResultDto<T>>();

        var httpResponse = await (await GetHttpClientAsync()).Value.GetAsync(ComposeUri(uri, getListRequestDto));

        var json = await httpResponse.Content.ReadAsStringAsync();
        if (json == "[]" || json.IsNullOrWhiteSpace()) return Ok(new ListResultDto<T>());
        
        if (getListRequestDto is IPagedRequestDto)
        {
            var pagedResultDto = json.ToType<PagedResultDto<T>>();
            return Result.Ok(new ListResultDto<T>(pagedResultDto.Items, pagedResultDto.TotalCount));
        }

        var listResultDto = new ListResultDto<T>(json.ToType<List<T>>());
        return Ok(listResultDto);
    }

    public async Task<Result<ListResultDto<T>>> UpdateAsync(string uri, TU updateInputDto)
    {
        try
        {
            var httpResponse = await (await GetHttpClientAsync())
                .Value.PutAsync($"{uri}", new StringContent(updateInputDto.ToJson(), Encoding.UTF8, "application/json"));

            var json = await httpResponse.Content.ReadAsStringAsync();
            if (json == "[]" || json.IsNullOrWhiteSpace()) return Ok(new ListResultDto<T>());

            if(json.StartsWith("{") && json.EndsWith("}"))
                return Ok(new ListResultDto<T>(new List<T> { json.ToType<T>() }));
            
            var items = json.ToType<List<T>>();
            
            return Ok(new ListResultDto<T>(items));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
        return Ok(new ListResultDto<T>());
    }

    public async Task<Result<ListResultDto<T>>> CreateAsync(string url, TC createInputDto)
    {
        try
        {
            var httpResponse = await (await GetHttpClientAsync())
                .Value.PostAsync($"{url}", new StringContent(createInputDto.ToJson(), Encoding.UTF8, "application/json"));

            var json = await httpResponse.Content.ReadAsStringAsync();
            if (json == "[]" || json.IsNullOrWhiteSpace()) return Ok(new ListResultDto<T>());

            if(json.StartsWith("{") && json.EndsWith("}"))
                return Ok(new ListResultDto<T>(new List<T> { json.ToType<T>() }));
            
            var items = json.ToType<List<T>>();
            
            return Ok(new ListResultDto<T>(items));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
        return Ok(new ListResultDto<T>());
        
    }

    public async Task<Result<T>> GetAsync(string uri)
    {
        var httpResponse = await (await GetHttpClientAsync()).Value.GetAsync(uri);
        var json = await httpResponse.Content.ReadAsStringAsync();
        return json.IsNullOrWhiteSpace() ? Result.Fail<T>(new Error("json is null")) : Ok(json.ToType<T>()) ;
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