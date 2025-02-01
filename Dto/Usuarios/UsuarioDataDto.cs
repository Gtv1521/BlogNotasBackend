using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEndNotes.Dto.Usuarios
{
    public class UsuarioDataDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdUser { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Minimo de caracteres es de 5")]
        public string? Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}