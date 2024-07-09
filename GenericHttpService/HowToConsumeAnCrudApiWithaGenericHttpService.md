# Create a generic HTTP Service to consume a CRUD API

## Introduction

In this blog post, I will show you how you can consume a **C# CRUD API** in by making use of a **Generic HTTP Service**

## Requirements

The following tools are needed to be able to run the solution.

- .NET 8.0 SDK
- VsCode, Visual Studio 2022 or another compatible IDE

In the [GitHub repo](https://github.com/bartvanhoey/AbpOpenIddictRepo) you find the **BookStoreWebApi** and **BookStoreConsole** applications that we will.

## Development

### Create a new .NET Core WEB API

```bash
    dotnet new webapi --use-controllers -o BookStoreWebApi
```

### Copy Data/Infra/Dtos folders

Copy/paste the Data/Infra and Dtos folder of the BookstoreWebApi sample project into the root of your project.

### Add a BooksController class to the Controllers folder

```csharp
using BookStoreWebApi.Data;
using BookStoreWebApi.Dtos.Books;
using BookStoreWebApi.Infra;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreWebApi.Controllers
{
    [Route("api/app/book")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public PagedResultDto<BookDto> Get() 
            => new() { Items = BookList.GetBooks, TotalCount = BookList.GetBooks.Count };

        [HttpGet("{id}")]
        public BookDto? Get(Guid id) 
            => BookList.GetBooks.FirstOrDefault(x => x.Id == id);

        
        [HttpPost]
        public BookDto Create([FromBody] CreateBookDto createBookDto)
        {
            var bookDto = new BookDto { Name = createBookDto.Name, Type = createBookDto.Type, Price = createBookDto.Price, PublishDate = createBookDto.PublishDate, Id = createBookDto.Id};
            BookList.GetBooks.Add(bookDto);
            return bookDto;
        }

        
        [HttpPut("{id}")]
        public BookDto? Put(Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            var bookDto = BookList.GetBooks.FirstOrDefault(x => x.Id == id);
            if (bookDto == null) return bookDto;
            bookDto.Name = updateBookDto.Name;
            bookDto.Price = updateBookDto.Price;
            bookDto.PublishDate = updateBookDto.PublishDate;
            bookDto.Type = updateBookDto.Type;
            return bookDto;
        }

        
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var bookDto = BookList.GetBooks.FirstOrDefault(x => x.Id == id);
            if (bookDto != null) BookList.GetBooks.Remove(bookDto);
        }
    }
}

```

### Run the API

 ![Swagger Api Endpoints BooksController](../images/swagger_bookscontroller.png )

### Create Generic HttpService

Get the [source code](https://github.com/bartvanhoey/AbpOpenIddictRepo) on GitHub.

Enjoy and have fun!
