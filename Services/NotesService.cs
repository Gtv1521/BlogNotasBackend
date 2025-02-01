using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using BackEndNotes.Models.Notes;

namespace BackEndNotes.Services
{
    public class NotesService
    {
        private readonly INotes<NotesModel, UpdateNoteDto> _note;
        public NotesService(INotes<NotesModel, UpdateNoteDto> note)
        {
            _note = note;
        }

        public async Task<NotesModel> ObtenerNota(string idUser)
        {
            return await _note.ViewOne(idUser);
        }
        public async Task<List<NotesModel>> ObtenerNotasUsuario(string idUser)
        {
            return await _note.ViewAllDataIdUser(idUser);
        }

        public async Task<string> CrearNota(NotesDto model)
        {
            return await _note.Create(new NotesModel {
                Title = model.Title,
                Contenido = model.Contenido,
                IdUser = model.IdUser,
                FechaCreacion = DateTime.Now
            });
        }

        public async Task<bool> RemoveNote(string id)
        {
            return await _note.Remove(id);
        }

    }
}