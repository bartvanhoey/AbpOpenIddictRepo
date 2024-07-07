namespace BookStoreAspNetCoreWebApi.Controllers.Infra
{
    public interface IHasTotalCount
    {
        long TotalCount { get; set; }
    }
}
