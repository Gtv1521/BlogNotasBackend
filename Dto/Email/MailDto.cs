using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Dto
{
    public class MailDto
    {
        [Required(ErrorMessage = "Debes enviar un email")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Mail { get; set; }
    }
}