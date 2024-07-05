using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BookStore.Authors
{
    public class AuthorAppService : BookStoreAppService, IAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;

        public AuthorAppService(IAuthorRepository authorRepository, AuthorManager authorManager)
        {
            _authorRepository = authorRepository;
            _authorManager = authorManager;
        }

     [Authorize(BookStorePermissions.Authors.Create)]
public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
{
    var author = await _authorManager.CreateAsync(
        input.Name,
        input.BirthDate,
        input.ShortBio
    );

    await _authorRepository.InsertAsync(author);

    return ObjectMapper.Map<Author, AuthorDto>(author);
}


        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorDto> GetAsync(Guid id)
        {
            var author = await _authorRepository.GetAsync(id);
            return ObjectMapper.Map<Author, AuthorDto>(Author);

        }

        public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Author.Name);
            }

            var authors = await _authorRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter
            );

            var totalCount = await AsyncExecuter.CountAsync(
                _authorRepository.WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    author => author.Name.Contains(input.Filter)
                )
            );

            return new PagedResultDto<AuthorDto>(
                totalCount,
                ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors)
            );
        }


        public Task UpdateAsync(Guid id, UpdateAuthorDto input)
        {
            throw new NotImplementedException();
        }
    }
}