using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces.Regulars;
using BackEndNotes.Models;

namespace BackEndNotes.Interfaces
{
    public interface INotes<T, S> : ICreated<T>, IViewOne<T>, IViewXId<T>, IRemove, IUpdate<UpdateNoteDto>
    {
        Task<long> CountNotes(string id);
        Task<bool> DeleteByIdUser(string id);
    }
}