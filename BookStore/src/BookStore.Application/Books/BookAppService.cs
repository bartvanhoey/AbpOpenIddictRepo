using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Books.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BookStore.Books
{
    public class BookAppService : CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto, CreateBookDto, UpdateBookDto>, IBookAppService
    {
        public BookAppService(IRepository<Book, Guid> repository): base(repository)
        {
            // GetPolicyName = BookStorePermissions.Books.Default;
            // GetListPolicyName = BookStorePermissions.Books.Default;
            // CreatePolicyName = BookStorePermissions.Books.Create;
            // UpdatePolicyName = BookStorePermissions.Books.Update;
            // DeletePolicyName = BookStorePermissions.Books.Delete;
        }
    }
}