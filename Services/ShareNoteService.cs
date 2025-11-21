using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Dto.Usuarios;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models.Notes;
using BlogNotasBackend.Interfaces.Principals;
using BlogNotasBackend.Models;
using BlogNotasBackend.requests;
using Microsoft.AspNetCore.WebUtilities;
using src.Interfaces.Principals;
using src.Models;
using src.Models.projeccions;

namespace src.Services
{
    public class ShareNoteService
    {
        private readonly IShare<ShareNoteModel, ShareNoteProjection> _collection;
        private readonly INotificationService<NotificationRequest, NotificationModel> _notify;
        private readonly INotes<NotesModel, UpdateNoteDto> _notes;
        private readonly IUsuario<UpdateUserDto, UsuarioDataDto> _user;
        public ShareNoteService(IUsuario<UpdateUserDto, UsuarioDataDto> user, INotes<NotesModel, UpdateNoteDto> notes, IShare<ShareNoteModel, ShareNoteProjection> collection, INotificationService<NotificationRequest, NotificationModel> notify)
        {
            _collection = collection;
            _notes = notes;
            _user = user;
            _notify = notify;
        }

        // obtiene una nota compartida 
        public async Task<ShareNoteModel> GetShare(string id)
        {
            return await _collection.ViewOne(id);
        }

        // muestra todas las notas compartidas que tiene un usuario
        public async Task<IEnumerable<ShareNoteProjection>> ShareNotes(string id)
        {
            return await _collection.GetShares(id);
        }

        // Obtiente varias notas comparidas para el usuario 
        public async Task<IEnumerable<ShareNoteProjection>> GetAllShare(string note, int page)
        {
            return await _collection.ViewAllDataIdUser(note, page);
        }

        // Comparte una nota con un usuario
        public async Task<string> NewShare(ShareNoteModel data)
        {
            var nota = await _notes.ViewOne(data.NoteId);
            data.IdLibreta = nota.IdLibreta;
            var response = await _collection.Create(data);
            var user = await _user.ViewOne(data.IdUser);
            var note = await _notes.ViewOne(data.NoteId);
            await _notify.SendNotificationAsync(new NotificationRequest
            {
                TargetId = data.NoteId,
                UserRefId = data.IdUserReference,
                Type = TypeNote.NotefyUser,
                Message = $"<strong>{user.Name}</strong> compartio la nota: <strong>{note.Title}</strong> contigo, ve a verla.",
                Title = $"Nota compartida",
                Data = new
                {
                    IdReference = response,
                },

            });
            return response;
        }

        // filtro para ver los datos de la nota en el id de usuario
        public async Task<IEnumerable<ShareNoteProjection>> Filter(string filter, string id)
        {
            return await _collection.Filter(filter, id);
        }

        //  actualiza permisos de nota compaertida
        public async Task<bool> UpdatePermits(string id, ShareNoteModel data)
        {
            return await _collection.UpdateData(id, data);
        }

        // elimina la referencia a una nota
        public async Task<bool> DeleteShare(string id)
        {
            return await _collection.Remove(id);
        }
    }
}