using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Notas_Back.Dto
{
    public class LogUserDto
    {   
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class Pass 
    {
        public string? Password { get; set; }
    }
}