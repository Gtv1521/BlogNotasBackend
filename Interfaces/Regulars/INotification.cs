using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Interfaces
{
    public interface INotification<T>
    {
        bool Notificar(T notification);
    }
}