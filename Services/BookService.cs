using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public async Task<string> Create(BooksDto libro)
        {
            var book = new LibreriasModel
            {
                Nombre = libro.NameBook,
                IdUser = libro.IdAuthor
            };
            return await _collection.Create(book);
        }

        public async Task<bool> Remove(string id)
        {
            return await _collection.Remove(id);
        }

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
    }
}