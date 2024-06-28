using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Books.Dtos
{
    public class CreateBookDto
    {

        [Required]
        [StringLength(128)]
        public string? Name { get; set; }

        [Required]

        public BookType Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate{ get; set;  }

        [Required]

        public float Price { get; set; }
    }
}