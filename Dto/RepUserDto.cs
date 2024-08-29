using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notas_Back.Dto
{
    public class RepUserDto
    {   [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int? IdUser { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}