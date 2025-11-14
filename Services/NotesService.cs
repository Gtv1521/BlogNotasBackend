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
using MongoDB.Bson;

namespace BackEndNotes.Services
{
    public class NotesService
    {
        private readonly INotes<NotesModel, UpdateNoteDto> _note;
        public NotesService(INotes<NotesModel, UpdateNoteDto> note)
        {
            _note = note;
        }

        // trae una nota 
        public async Task<NotesModel> ObtenerNota(string idUser)
        {
            return await _note.ViewOne(idUser);
        }

        // trae lista de notas por libreta
        public async Task<List<NotesModel>> ObtenerNotasUsuario(string IdLibreta, int pagina)
        {
            return await _note.ViewAllDataIdUser(IdLibreta, pagina);
        }

        // cuenta la cantidad de notas en una libreta
        public async Task<long> CountNotes(string idNota)
        {
            return await _note.CountNotes(idNota);
        }
        // crea una nota nueva
        public async Task<string> CrearNota(NotesDto model)
        {
            return await _note.Create(new NotesModel
            {
                Title = model.Title,
                Contenido = model.Contenido,
                IdUser = ObjectId.Parse(model.IdUser),
                IdLibreta = ObjectId.Parse(model.IdLibreta),
                FechaCreacion = DateTime.Now,
                FechaUpdate = DateTime.Now,
            });
        }

        // filtro
        public async Task<IEnumerable<NotesModel>> Filter(string filter, string id)
        {
            return await _note.Filter(filter, id);
        }


        // actualiza datos de una nota 
        public async Task<bool> ActualizarNota(string id, UpdateNoteDto model)
        {
            return await _note.UpdateData(id, model);
        }

        // elimina una nota de la libreta 
        public async Task<bool> RemoveNote(string id)
        {
            return await _note.Remove(id);
        }

        //  makes a change of book in the note
        public async Task<bool> ChangeBook(string idNote, string idLibreta)
        {
            return await _note.ChangeBook(idNote, idLibreta);
        }
    }
}