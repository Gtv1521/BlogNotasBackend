using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogNotasBackend.Models
{
    // almacena datos de session de usuario
    public class SessionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdSession { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId IdUser { get; set; }
        public string TokenRefresh { get; set; }
    }
}