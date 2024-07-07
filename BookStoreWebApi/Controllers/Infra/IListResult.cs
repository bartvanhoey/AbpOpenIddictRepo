namespace BookStoreAspNetCoreWebApi.Controllers.Infra;

public interface IListResult<T>
{
    IReadOnlyList<T> Items { get; set; }
}