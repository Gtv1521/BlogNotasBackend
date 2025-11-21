using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogNotasBackend.Models
{
    public class ShareNoteModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // id 
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; } // usuario que creo la nota
        [BsonRepresentation(BsonType.ObjectId)]
        public string NoteId { get; set; } // id nota 
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdLibreta { get; set; } // id libreta
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUserReference { get; set; } // usuario de referencia 
        public bool WritePermits { get; set; } // Permiso de escribir 
        public bool ReadPermits { get; set; } // permiso de leer 
    }
}