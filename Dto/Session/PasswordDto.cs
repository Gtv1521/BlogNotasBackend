using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEndNotes.Dto
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "el id de usuario es requerido")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdUser { get; set; }
        
        [Required(ErrorMessage = "el Passwprd es requerido")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}