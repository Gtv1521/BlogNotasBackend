using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Usuarios;

namespace BackEndNotes.Dto
{
    public class UserResDto : Id
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}