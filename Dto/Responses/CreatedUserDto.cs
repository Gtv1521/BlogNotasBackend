using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notas_Back.Dto;

namespace NotasBack.Dto
{
    public class CreatedUserDto : NoData
    {
        public string? Token { get; set; }
    }
}