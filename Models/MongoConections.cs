using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Notas_Back.Models
{
    public class MongoConections
    {
        public string? ConnectionStrings { get; set; }
        public string? DatabaseName { get; set; }


    }
}