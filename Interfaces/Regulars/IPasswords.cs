using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface IPasswords<T>
    {
        Task<bool> ChoosePassword(T model);
    }
}