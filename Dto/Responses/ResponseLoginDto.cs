using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotasBack.Dto.Responses
{
    public class ResponseLoginDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Mensaje { get; set; }
        public string? Token { get; set; }

    }
}