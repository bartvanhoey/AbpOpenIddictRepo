using BookStoreMaui.Models;

namespace BookStoreMaui.Services.Books.Dtos;

public class UpdateBookDto
{
    public UpdateBookDto(Guid id, BookType type, float price, DateTime publishDate, string? name)
    {
        Id = id;
        Type = type;
        Price = price;
        PublishDate = publishDate;
        Name = name;
    }

    public Guid Id { get; set; }
    public BookType Type { get; set; }
    public float Price { get; set; }
    public DateTime PublishDate { get; set; }
    public string? Name { get; set; }
}