using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Amazon.Util;
using BackEndNotes.Dto.Books;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models.Librerias;
using BackEndNotes.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackEndNotes.Collections
{
    public class LibretaCollection : ILibreta<LibreriasModel>
    {
        private readonly IMongoCollection<LibreriasModel> _collection;
        public LibretaCollection(Context context)
        {
            _collection = context.GetCollection<LibreriasModel>("Libretas");
        }

        // create one new note
        public async Task<string> Create(LibreriasModel Object)
        {
            await _collection.InsertOneAsync(Object);
            return Object.IdLibreta;
        }

        // Coynt the number of books of a user
        public async Task<long> CountLibretas(string idUser)
        {
            var filter = Builders<LibreriasModel>.Filter.Eq(book => book.IdUser, idUser);
            return await _collection.CountDocumentsAsync(filter);
        }

        //  delete one book
        public async Task<bool> Remove(string id)
        {
            var filter = Builders<LibreriasModel>.Filter.Eq(book => book.IdLibreta, id);
            var response = await _collection.DeleteManyAsync(filter);
            if (response.IsAcknowledged && response.DeletedCount > 0) return response.IsAcknowledged;
            return false;
        }

        //  update data one book
        public async Task<bool> UpdateData(string id, LibreriasModel name)
        {
            var filtro = Builders<LibreriasModel>.Filter.Where(u => u.IdLibreta == id);

            var actualizacion = Builders<LibreriasModel>.Update
                .Set(u => u.Nombre, name.Nombre)
                .Set(u => u.UpdateBook, DateTime.Now);

            var response = await _collection.UpdateOneAsync(filtro, actualizacion);

            return response.IsAcknowledged;
        }

        // list all books of user
        public async Task<List<LibreriasModel>> ViewAllDataIdUser(string userId, int pagina)
        {
            var cantidad = 10;
            var filter = Builders<LibreriasModel>.Filter.Eq(book => book.IdUser, userId);
            return await _collection.Find(filter)
            .SortByDescending(n => n.UpdateBook)
            .Skip((pagina - 1) * cantidad)  // cantidad de datos que salta 
            .Limit(cantidad)  // cantidad de datos que va a traer 
            .ToListAsync(); // hace una lista de datos y responde 
        }

        public async Task<LibreriasModel> ViewOne(string id)
        {
            var filter = Builders<LibreriasModel>.Filter.Eq("_id", new ObjectId(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}