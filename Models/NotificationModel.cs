using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace src.Models
{
    public class NotificationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUserReference { get; set; }
        public bool IsRead { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TargetUserId { get; set; } // usuerio dueño de la nota
        public string TargetUserName { get; set; } // nombre de usuario dueño de la nota
        public DateTime AtCreated { get; set; } // fecha de creacion de notification 
    }
}