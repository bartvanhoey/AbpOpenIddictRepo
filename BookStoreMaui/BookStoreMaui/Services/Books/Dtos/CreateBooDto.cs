using BookStoreMaui.Models;

namespace BookStoreMaui.Services.Books.Dtos;

public  class CreateBooDto
{
    public CreateBooDto(string? name, BookType bookType, DateTime publishDate, float price)
    {
        Name = name;
        PublishDate = publishDate;
        Type = bookType;
        Price = price;
    }

    public BookType Type { get; set; }
    public float Price { get; set; }
    public DateTime PublishDate { get; set; }
    public string? Name { get; set; }
}