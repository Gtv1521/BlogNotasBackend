using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface IViewOne<T>
    {
        Task<T> ViewOne(string Dato);
    }
}