﻿using AutoMapper;
using BookStore.Books;
using BookStore.Books.Dtos;

namespace BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

         CreateMap<Book, BookDto>();
         CreateMap<CreateBookDto, Book>();
         CreateMap<UpdateBookDto, Book>();
    }
}
