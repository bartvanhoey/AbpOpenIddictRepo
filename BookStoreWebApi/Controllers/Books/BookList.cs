﻿using BookStoreAspNetCoreWebApi.Controllers.Books.Dtos;

namespace BookStoreAspNetCoreWebApi.Controllers.Books
 {
    public static class  BookList
    {
        public static readonly List<BookDto> GetBooks;

        static BookList()
        {
            GetBooks = new List<BookDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "1984",
                    Type = BookType.Dystopia,
                    PublishDate = new DateTime(1949, 6, 8),
                    Price = 19.84f
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(1995, 9, 27),
                    Price = 42.0f
                },
            };
        }
    }
}
