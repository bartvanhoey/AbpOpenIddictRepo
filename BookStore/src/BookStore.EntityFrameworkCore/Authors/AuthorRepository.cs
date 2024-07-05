using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BookStore.Authors
{
    public class AuthorRepository : EfCoreRepository<BookStoreDbContext, Author, Guid>, IAuthorRepository
    {
    public AuthorRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

        public async Task<Author?> FindByNameAsync(string name)
        {
            return await (await GetDbSetAsync()).FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Author>> GetListAsync(int skipCount, int maxResultCount, string sorting, string? filter = null)
        {
            var dbSet = await GetDbSetAsync();

            
            
            return await dbSet.
                WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    x => x.Name!= null && x.Name.Contains(filter))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}