using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface IViewXId<T>
    {
        Task<List<T>> ViewAllDataIdUser(string userId);  
    }
}