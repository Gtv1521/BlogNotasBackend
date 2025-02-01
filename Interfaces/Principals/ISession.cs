using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces.Principals
{
    public interface ISessionUser<T, P, M> : ICreated<T>, ILognIn<T>, IPasswords<P>, INotification<M>
    {
   
    }
}