using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}