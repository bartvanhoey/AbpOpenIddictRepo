# Create a generic HTTP Service to consume a CRUD API

## Introduction

In this step-by-step tutorial, I will show you how you can create a **generic HTTP Service** that consumes a **.NET Core WEB API**.

To simplify things, we will create a standard **.NET Core WEB API** (BookStoreWebApi) and a **.NET Core Console** (BookStoreConsole) application to implement a **generic HTTP Service** that consumes a **C# CRUD API**.

## Requirements

- .NET 8.0 SDK
- VsCode, Visual Studio 2022 or another compatible IDE

In the [GitHub repo](https://github.com/bartvanhoey/AbpOpenIddictRepo) you can find the **BookStoreWebApi** and **BookStoreConsole**

## Development

### Create a new .NET Core WEB API

Let's first create a .NET Core WEB API with a BooksController with the standard CRUD API endpoints.

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

![Swagger Api Endpoints BooksController](../images/swagger_bookscontroller.png)

## Create a .NET Console app

```bash
    dotnet new console -o BookStoreConsole
```

### Create a IHttpService interface

Create a **IHttpService interface** with the standard CRUD method definitions in the **Http** folder

```csharp
public interface IHttpService<T, in TC, in TU, in TG, in TD>
{
    Task<ListResultDto<T>> GetListAsync(string uri, TG? getListRequestDto = default);
    Task<ListResultDto<T>> UpdateAsync(string uri, TU updateInputDto);
    Task<T> CreateAsync(string uri, TC createInputDto);
    Task<T> GetAsync(string uri);
    Task DeleteAsync(string uri, TD id);
}
```

Copy/Paste the **Infra** folder of the BookStoreConsole sample application into the **Http** folder

Create a **HttpService class** in the **Http** folder that implements the **IHttpService interface**

```csharp
public class HttpService<T, TC, TU, TL, TD>(ISecureStorageService storageService)
    : HttpServiceBase<T, TC, TU, TL, TD>(storageService), IHttpService<T, TC, TU, TL, TD>
    where T : class
    where TC : class
    where TU : class
    where TL : class
{
    public async Task<ListResultDto<T>> GetListAsync(string uri, TL? getListRequestDto = default)
    {
        if (getListRequestDto == null) return new ListResultDto<T>();

        var httpResponse = await (await GetHttpClientAsync()).Value.GetAsync(ComposeUri(uri, getListRequestDto));

        var json = await httpResponse.Content.ReadAsStringAsync();
        if (json == "[]" || json.IsNullOrWhiteSpace()) return new ListResultDto<T>();

        if (getListRequestDto is IPagedRequestDto)
        {
            var pagedResultDto = json.ToType<PagedResultDto<T>>();
            return new PagedResultDto<T>(pagedResultDto.TotalCount,pagedResultDto.Items);
        }

        var listResultDto = new ListResultDto<T>(json.ToType<List<T>>());
        return listResultDto;
    }

    public async Task<ListResultDto<T>> UpdateAsync(string uri, TU updateInputDto)
    {

        var httpResponse = await (await GetHttpClientAsync())
            .Value.PutAsync($"{uri}", new StringContent(updateInputDto.ToJson(), Encoding.UTF8, "application/json"));

        var json = await httpResponse.Content.ReadAsStringAsync();
        if (json == "[]" || json.IsNullOrWhiteSpace()) return new ListResultDto<T>();

        if (json.StartsWith("{") && json.EndsWith("}"))
            return new ListResultDto<T>(new List<T> { json.ToType<T>() });

        var items = json.ToType<List<T>>();

        return new ListResultDto<T>(items);
    }

    public async Task<T> CreateAsync(string uri, TC createInputDto)
    {
        var httpResponse = await (await GetHttpClientAsync())
            .Value.PostAsync(uri, new StringContent(createInputDto.ToJson(), Encoding.UTF8, "application/json"));

        var json = await httpResponse.Content.ReadAsStringAsync();
        return json.ToType<T>();
    }

    public async Task<T> GetAsync(string uri)
    {
        var httpResponse = await (await GetHttpClientAsync()).Value.GetAsync(uri);
        var json = await httpResponse.Content.ReadAsStringAsync();
        return json.ToType<T>();
    }

    public async Task DeleteAsync(string uri, TD id)
    {
        var httpResponse = await (await GetHttpClientAsync()).Value.DeleteAsync($"{uri}/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Failed to delete");
        }
    }
}

```

### Create a IBookService interface

```csharp
public interface IBookService
{
    Task<IEnumerable<BookDto>> GetBooksAsync();
    Task<BookDto?> CreateBookAsync(CreateBookDto bookDto);
    // other method definitions here ...
}
```

```csharp
public class BookService(IHttpService<BookDto, CreateBookDto, UpdateBookDto, GetBooksPagedRequestDto, Guid> httpService, string bookApiUrl) : IBookService
{
    public async Task<IEnumerable<BookDto>> GetBooksAsync()
        => (await httpService.GetListAsync($"{bookApiUrl}", new GetBooksPagedRequestDto())).Items;

    public async Task<BookDto?> CreateBookAsync(CreateBookDto bookDto)
        => await httpService.CreateAsync($"{bookApiUrl}", bookDto);
    // other methods here ...
}
```







Get the [source code](https://github.com/bartvanhoey/AbpOpenIddictRepo) on GitHub.

Enjoy and have fun!
