namespace BookStoreWebApi.Models;

public interface IPagedResult<T>: IListResult<T>, IHasTotalCount
{
        
}