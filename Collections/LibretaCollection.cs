using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models.Librerias;
using BackEndNotes.Utils;
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

        public async Task<string> Create(LibreriasModel Object)
        {
            try
            {
                await _collection.InsertOneAsync(Object);
                return Object.IdLibreta;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        //  delete one book
        public async Task<bool> Remove(string id)
        {
            var filter = Builders<LibreriasModel>.Filter.Eq(book => book.IdLibreta, id);
            var response = await _collection.DeleteOneAsync(filter);
            if (response.IsAcknowledged && response.DeletedCount > 0) return response.IsAcknowledged;
            return false;
        }
    
    //  update data one book
        public Task<bool> UpdateData(string id, LibreriasModel model)
        {
            throw new NotImplementedException();
        }

        // list all books of user
        public Task<List<LibreriasModel>> ViewAllDataIdUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}