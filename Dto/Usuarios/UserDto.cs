using System.ComponentModel.DataAnnotations;

namespace BackEndNotes.Dto
{
    public class UserDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? Name { get; set; } 

        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "El rol de usuario es obligatorio.")]
        public string? Role { get; set; }
    }
}