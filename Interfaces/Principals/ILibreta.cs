using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces.Regulars;
using BlogNotasBackend.Interfaces.Principals;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Interfaces.Principals
{
    public interface ILibreta<T> : ICreated<T>, IFilter<T>, IRemove, IUpdate<T>, IViewXId<T>, IViewOne<T>
    {
        Task<long> CountLibretas(string idUser);
        Task<T> SearchCollection(string idUser, string name);
    }
}