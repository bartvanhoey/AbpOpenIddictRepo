using BookStoreAspNetCoreWebApi.Controllers.Authors.Dtos;
using BookStoreAspNetCoreWebApi.Controllers.Infra;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAspNetCoreWebApi.Controllers.Authors;

[Route("api/app/author")]
[ApiController]
public class AuthorsController : ControllerBase
{
    [HttpGet]
    public PagedResultDto<AuthorDto> Get() 
        => new() { Items = AuthorList.GetAuthors, TotalCount = AuthorList.GetAuthors.Count };

    [HttpGet("{id}")]
    public AuthorDto? Get(Guid id) 
        => AuthorList.GetAuthors.FirstOrDefault(x => x.Id == id);

        
    [HttpPost]
    public AuthorDto Create([FromBody] CreateAuthorDto createAuthorDto)
    {
        var authorDto = new AuthorDto { Name = createAuthorDto.Name, BirthDate = createAuthorDto.BirthDate, ShortBio = createAuthorDto.ShortBio, Id = createAuthorDto.Id};
        AuthorList.GetAuthors.Add(authorDto);
        return authorDto;
    }

        
    [HttpPut("{id}")]
    public AuthorDto? Put(Guid id, [FromBody] UpdateAuthorDto updateAuthorDto)
    {
        var authorDto = AuthorList.GetAuthors.FirstOrDefault(x => x.Id == id);
        if (authorDto == null) return authorDto;
        authorDto.Name = updateAuthorDto.Name;
        authorDto.BirthDate = updateAuthorDto.BirthDate;
        authorDto.ShortBio = updateAuthorDto.ShortBio;
        return authorDto;
    }

        
    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var authorDto = AuthorList.GetAuthors.FirstOrDefault(x => x.Id == id);
        if (authorDto != null) AuthorList.GetAuthors.Remove(authorDto);
    }
}