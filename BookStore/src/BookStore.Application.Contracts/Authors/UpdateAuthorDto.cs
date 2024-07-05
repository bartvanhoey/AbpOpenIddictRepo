using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Authors
{
    public class UpdateAuthorDto
    {

        [Required]
        [StringLength(64)]
        public string? Name { get; set; }


        [Required]
        public DateTime BirthDate { get; set; }

        public string? ShortBio { get; set; }
    }
}
