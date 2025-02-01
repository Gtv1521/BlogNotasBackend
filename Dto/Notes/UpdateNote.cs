using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Dto.Notes
{
    public class UpdateNoteDto
    {
        public string? Title { get; set; }
        public string? Contenido { get; set; }
        [Required(ErrorMessage = "El id de usuario es requerido")]
        public string? IdUser { get; set; }
        public DateTime FechaUpdate { get; set; }
    }
}