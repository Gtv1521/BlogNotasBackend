using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces.Regulars;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Interfaces.Principals
{
    public interface ILibreta<T> : ICreated<T>, IRemove, IUpdate<T>, IViewXId<T>
    {
        Task<long> CountLibretas(string idUser);
    }
}