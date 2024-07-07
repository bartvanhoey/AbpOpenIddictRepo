namespace BookStoreAspNetCoreWebApi.Controllers.Infra;

public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T> {  }