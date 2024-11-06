using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Notas_Back.Models
{
    public class UsuariosM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public  string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirsName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Rol { get; set; }
    }
}