using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using BackEndNotes.Dto.Books;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models.Librerias;

namespace BackEndNotes.Services
{
    public class BookService
    {
        private readonly ILibreta<LibreriasModel> _collection;
        public BookService(ILibreta<LibreriasModel> collection)
        {
            _collection = collection;
        }
        // Crea una nueva libreta 
        public async Task<string> Create(BooksDto libro)
        {
            var book = new LibreriasModel
            {
                Nombre = libro.NameBook,
                IdUser = libro.IdAuthor
            };
            return await _collection.Create(book);
        }

        //  elimina una libreta 
        public async Task<bool> Remove(string id)
        {
            return await _collection.Remove(id);
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

        // update name book
        public async Task<bool> UpdateBook(string id, string name)
        {
            var dato = new LibreriasModel { Nombre = name};
            return await _collection.UpdateData(id, dato);
        }
    }
}