using System;
using System.ComponentModel.DataAnnotations;

public class CreateAuthorDto
{

    [Required]
    [StringLength(64)]
    public string? Name { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    public string? ShortBio { get; set; }
}

    
