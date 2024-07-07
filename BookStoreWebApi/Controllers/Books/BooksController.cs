﻿using BookStoreAspNetCoreWebApi.Controllers.Books.Dtos;
using BookStoreAspNetCoreWebApi.Controllers.Infra;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAspNetCoreWebApi.Controllers.Books
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
            var bookDto = new BookDto { Name = createBookDto.Name, Type = createBookDto.Type, Price = createBookDto.Price, PublishDate = createBookDto.PublishDate, Id = new Guid()};
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