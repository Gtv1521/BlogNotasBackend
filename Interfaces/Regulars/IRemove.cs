using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces.Regulars
{
    public interface IRemove
    {
        Task<bool> Remove(string id);
    }
}