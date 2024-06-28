namespace BookStoreMaui.Services.Http;

public interface IPagedListDto
{
    int SkipCount { get; set; }
    int MaxResultCount { get; set; }
    string Sorting { get; set; }
    string Filter { get; set; }
}