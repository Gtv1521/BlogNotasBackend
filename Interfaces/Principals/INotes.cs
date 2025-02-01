using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces.Regulars;

namespace BackEndNotes.Interfaces
{
    public interface INotes<T, S> : ICreated<T>, IViewOne<T>, IViewXId<T>, IRemove
    {
        Task<bool> Update(S model);
    }
}