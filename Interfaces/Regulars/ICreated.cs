using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface ICreated<T>
    {
        Task<string> Create(T Object);
    }
}