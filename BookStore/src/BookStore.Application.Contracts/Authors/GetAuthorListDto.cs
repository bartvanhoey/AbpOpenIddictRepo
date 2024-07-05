using Volo.Abp.Application.Dtos;

public class GetAuthorListDto :  PagedAndSortedResultRequestDto
    {
        public string? Filter{ get; set; }  
    }
