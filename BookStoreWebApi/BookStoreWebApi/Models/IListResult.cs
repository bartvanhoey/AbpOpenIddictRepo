namespace BookStoreWebApi.Models;

public interface IListResult<T>
{
    IReadOnlyList<T> Items { get; set; }
}