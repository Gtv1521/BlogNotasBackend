using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notas_Back.Dto;
using Notas_Back.Models;

namespace Notas_Back.Interfaces
{
    public interface ICrud<T>
    {
        Task<T> Get(string Id);
        Task Post(T modelo);
        Task Update(T modelo);
        Task Delete(string Id);
    }
}