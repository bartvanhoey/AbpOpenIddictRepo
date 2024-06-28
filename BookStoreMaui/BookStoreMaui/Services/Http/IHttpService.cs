using FluentResults;

namespace BookStoreMaui.Services.Http;

public interface IHttpService<T, in TC, in TU, in TG>
{
    Task<Result<ListResult<T>>> GetListAsync(string uri, TG? getListInputDto = default);
    Task<Result<ListResult<T>>> UpdateAsync(string uri, TU updateInputDto);
    Task<Result<ListResult<T>>> CreateAsync(string url, TC createInputDto);
    Task<Result<T>> GetAsync(string uri);
}

public class ListResult<T>
{
    public ListResult()
    {
            
    }
        
    public ListResult(List<T> items )
    {
        Items = items;
        TotalCount = items?.Count ?? 0;
    }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = new();
}