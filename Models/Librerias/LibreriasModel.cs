using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEndNotes.Models.Librerias
{
    public class LibreriasModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdLibreta { get; set; }
        public string? Nombre { get; set; }
        public string? IdUser { get; set; }
        public DateTime UpdateBook { get; set; }
        public DateTime CreateBook { get; set; }
    }
}