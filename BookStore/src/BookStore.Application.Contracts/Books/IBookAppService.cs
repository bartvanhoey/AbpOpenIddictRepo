using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Books.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BookStore.Books
{
    public interface IBookAppService : ICrudAppService<BookDto, Guid, PagedAndSortedResultRequestDto, CreateBookDto, UpdateBookDto>, IApplicationService
    {
        Task CreateManyAsync(IEnumerable<CreateBookDto> input);

    }

    public class MyBookAppService : IBookAppService
    {
        public Task<BookDto> CreateAsync(CreateBookDto input)
        {
            throw new NotImplementedException();
        }

        public Task CreateManyAsync(IEnumerable<CreateBookDto> input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input)
        {
            throw new NotImplementedException();
        }
    }







}