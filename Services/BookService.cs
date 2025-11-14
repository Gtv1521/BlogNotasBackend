using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using BackEndNotes.Dto.Books;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models.Librerias;
using BackEndNotes.Models.Notes;

namespace BackEndNotes.Services
{
    public class BookService
    {
        private readonly ILibreta<LibreriasModel> _collection;
        private readonly INotes<NotesModel, UpdateNoteDto> _note;
        public BookService(ILibreta<LibreriasModel> collection, INotes<NotesModel, UpdateNoteDto> note)
        {
            _collection = collection;
            _note = note;
        }
        // Crea una nueva libreta 
        public async Task<string> Create(BooksDto libro)
        {
            var book = new LibreriasModel
            {
                Nombre = libro.NameBook,
                IdUser = libro.IdAuthor,
                CreateBook = DateTime.Now,
                UpdateBook = DateTime.Now,
            };
            return await _collection.Create(book);
        }

        public async Task<long> Countlibreta(string idUser)
        {
            return await _collection.CountLibretas(idUser);
        }

        //  filtra las libretas 
        public async Task<IEnumerable<LibreriasModel>> Filter(string filter, string id)
        {
            return await _collection.Filter(filter, id);
        }
        
        //  elimina una libreta 
        public async Task<bool> Remove(string id)
        {
            var count = await _note.CountNotes(id); // trae el mumero de notas
            if (count > 0) await _note.DeleteByIdUser(id); // Elimina todas las notas del usuario
            return await _collection.Remove(id); // elimina el libro 
        }

        // muestra todos los datos de una libreta 
        public LibreriasModel ViewAllData()
        {
            var response = new LibreriasModel
            {
                IdLibreta = "adsaijdsaiojfa",
                Nombre = "Todas las Libretas",
                IdUser = "asdasdasd"
            };
            return response;
        }

        // view all books of user
        public async Task<List<LibreriasModel>> ViewAllBooks(string IdUser, int pagina)
        {
            return await _collection.ViewAllDataIdUser(IdUser, pagina);
        }

        // get one book
        public async Task<LibreriasModel> ViewOneBook(string id)
        {
            return await _collection.ViewOne(id);
        }

        // update name book
        public async Task<bool> UpdateBook(string id, string name)
        {
            var dato = new LibreriasModel { Nombre = name };
            return await _collection.UpdateData(id, dato);
        }
    }
}