using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Dto.Usuarios;
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
        public async Task<List<NotesModel>> ObtenerNotasUsuario(string IdLibreta, int pagina)
        {
            return await _note.ViewAllDataIdUser(IdLibreta, pagina);
        }

        public async Task<string> CrearNota(NotesDto model)
        {
            return await _note.Create(new NotesModel
            {
                Title = model.Title,
                Contenido = model.Contenido,
                IdUser = model.IdUser,
                IdLibreta = model.IdLibreta,
                FechaCreacion = DateTime.Now
            });
        }

        public async Task<bool> ActualizarNota(string id, UpdateNoteDto model)
        {
            return await _note.UpdateData(id, model);
        }

        public async Task<bool> RemoveNote(string id)
        {
            return await _note.Remove(id);
        }

    }
}