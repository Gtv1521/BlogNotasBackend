using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface IViews<T>
    {
        Task<IEnumerable<T>> ViewsAll();
        Task<T> ViewsById(string id);
        Task<bool> Create(T Object);
    }
}