using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notas_Back.Models
{
    public class Notas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdNota { get; set; }
        public string? IdUser { get; set; }
        public string? NameUser { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public DateTime FechaCreada { get; set; }
    }
}