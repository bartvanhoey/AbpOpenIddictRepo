using BookStoreMaui.Services.Http.Infra;

namespace BookStoreMaui.Services.Http;

public interface IHttpService<T, in TC, in TU, in TG, in TD>
{
    Task<ListResultDto<T>> GetListAsync(string uri, TG? getListRequestDto = default);
    Task<ListResultDto<T>> UpdateAsync(string uri, TU updateInputDto);
    Task<ListResultDto<T>> CreateAsync(string uri, TC createInputDto);
    Task<T> GetAsync(string uri);
    Task DeleteAsync(string uri, TD id);
}