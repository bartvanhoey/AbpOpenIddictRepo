namespace BookStoreMaui.Services.Http.Infra;

public class ListResultDto<T> : IListResult<T>, IHasTotalCount
{
    public IReadOnlyList<T> Items
    {
        get { return _items ??= new List<T>(); }
        set => _items = value;
    }

    public long TotalCount { get; set; }

    private IReadOnlyList<T>? _items;

    public ListResultDto()
    {
    }

    public ListResultDto(IReadOnlyList<T> items)
    {
        Items = items;
        TotalCount =  items.Count;
    }

    public ListResultDto(IReadOnlyList<T> items, long totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }
}