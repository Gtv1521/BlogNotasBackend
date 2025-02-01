using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEndNotes.Dto.Usuarios
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }

        public string? Role { get; set; }
    }
}