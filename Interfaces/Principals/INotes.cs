using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces.Regulars;
using BackEndNotes.Models;
using BlogNotasBackend.Interfaces.Principals;

namespace BackEndNotes.Interfaces
{
    public interface INotes<T, S> : ICreated<T>, IViewOne<T>, IFilter<T>, IViewXId<T>, IRemove, IUpdate<UpdateNoteDto>
    {
        Task<long> CountNotes(string id);
        Task<bool> DeleteByIdUser(string id);
        Task<bool> ChangeBook(string IdNote, string IdLibreta);
    }
}