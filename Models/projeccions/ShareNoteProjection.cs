using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Models;
using BackEndNotes.Models.Notes;
using BlogNotasBackend.Models;

namespace src.Models.projeccions
{
    public class ShareNoteProjection : ShareNoteModel
    {
        public List<UserModel> UserDetails { get; set; }
        public List<NotesModel> NoteDetails { get; set; }
    }
}