using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotasBack.Dto.entrance
{
    public class updateuser : IdDto
    {
        public string? Email { get; set; }
        public string? FirsName { get; set; }
        public string? LastName { get; set; }

    }
}