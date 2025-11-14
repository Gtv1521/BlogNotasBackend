using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Regulars;
using BlogNotasBackend.Interfaces.Principals;

namespace src.Interfaces.Principals
{
    public interface IShare<T, P> : ICreated<T>, IViewOne<T>, IViewXId<P>, IUpdate<T>, IRemove
    {
        Task<long> Count(string id);
        Task<IEnumerable<P>> Filter(string filter, string id);       
    }
}