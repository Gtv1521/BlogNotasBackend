using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Models;

namespace BackEndNotes.Interfaces
{
    public interface ICreated<T>
    {
        Task<bool> Create(T Object); 
    }
}