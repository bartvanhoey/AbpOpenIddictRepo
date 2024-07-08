namespace BookStoreWebApi.Models;

public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T> {  }