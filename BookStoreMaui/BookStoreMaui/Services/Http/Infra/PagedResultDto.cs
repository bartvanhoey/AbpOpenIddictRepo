namespace BookStoreMaui.Services.Http.Infra;

public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
{
    public long TotalCount { get; set; }
}