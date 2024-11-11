using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notas_Back.Dto
{
    public class NotasUpdateDto
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public DateTime FechaUpdate { get; set; }

    }
}