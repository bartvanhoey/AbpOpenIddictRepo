namespace BookStoreAspNetCoreWebApi.Controllers.Infra;

public interface IPagedResult<T>: IListResult<T>, IHasTotalCount
{
        
}