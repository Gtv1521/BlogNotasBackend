using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEndNotes.Models.Notes
{
    public class NotesModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdNote { get; set; }
        public string? Title { get; set; }
        public string? Contenido { get; set; }
        [Required]
        public string? IdUser { get; set; }
        public string? IdLibreta { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaUpdate { get; set; }

    }
}