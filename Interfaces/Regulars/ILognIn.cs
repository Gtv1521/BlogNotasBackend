using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface ILognIn<T>
    {
         Task<T> Login(string username, string password);
    }
}