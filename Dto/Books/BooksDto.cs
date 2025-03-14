using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Dto.Books
{
    public class BooksDto
    {
        [Required(ErrorMessage = "El name is requiered")]
        public string? NameBook { get; set; }

        [Required(ErrorMessage = "El id of author is requiered")]
        public string? IdAuthor { get; set; }
    }
} 