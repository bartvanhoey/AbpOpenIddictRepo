using BookStoreMaui.Services.Http.Infra;
using FluentResults;

namespace BookStoreMaui.Services.Http;

public interface IHttpService<T, in TC, in TU, in TG>
{
    Task<Result<ListResultDto<T>>> GetListAsync(string uri, TG? getListRequestDto = default);
    Task<Result<ListResultDto<T>>> UpdateAsync(string uri, TU updateInputDto);
    Task<Result<ListResultDto<T>>> CreateAsync(string url, TC createInputDto);
    Task<Result<T>> GetAsync(string uri);
}