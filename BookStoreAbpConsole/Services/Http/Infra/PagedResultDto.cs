namespace BookStoreAbpConsole.Services.Http.Infra;

public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
{
    
}