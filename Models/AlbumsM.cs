using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotasBack.Models
{
    public class AlbumsM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id  { get; set; }
        public string? IdUser  { get; set; }
        public string? NameAlbum  { get; set; }
    }
}