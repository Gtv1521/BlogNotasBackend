using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEndNotes.Dto.Notes
{
    public class NotesDto
    {
        
        public string? Title { get; set; }
        public string? Contenido { get; set; }
        [Required(ErrorMessage ="El id de usuario es requerido")]
        public string? IdUser { get; set; }
        public string? IdLibreta { get; set; }
    }
}