using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces.Regulars
{
    public interface IUpdate<T>
    {
        Task<bool> UpdateData(string id, T model);
    }
}