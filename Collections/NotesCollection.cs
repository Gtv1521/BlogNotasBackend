using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces;
using BackEndNotes.Models.Librerias;
using BackEndNotes.Models.Notes;
using BackEndNotes.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace BackEndNotes.Collections
{
    public class NotesCollection : INotes<NotesModel, UpdateNoteDto>
    {
        private readonly IMongoCollection<NotesModel> _database;
        private readonly IMongoCollection<LibreriasModel> _collection;
        public NotesCollection(Context context)
        {
            _database = context.GetCollection<NotesModel>("Notas");
            _collection = context.GetCollection<LibreriasModel>("Libretas");
        }
        public async Task<string> Create(NotesModel Object)
        {
            try
            {
                await _database.InsertOneAsync(Object);
                var response = await UpdateDate(Object.IdLibreta);
                return Object.IdNote;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        // cuenta las notas que hay en una nota
        public async Task<long> CountNotes(string id)
        {
            var filter = Builders<NotesModel>.Filter.Eq(n => n.IdLibreta, id);
            return await _database.CountDocumentsAsync(filter);
        }

        public async Task<bool> Remove(string id)
        {
            var filter = Builders<NotesModel>.Filter.Eq(note => note.IdNote, id);
            var result = await _database.DeleteManyAsync(filter);
            if (result.IsAcknowledged && result.DeletedCount > 0) return result.IsAcknowledged;
            return false;
        }

        public async Task<bool> UpdateData(string id, UpdateNoteDto model)
        {
            var filter = Builders<NotesModel>.Filter.Eq(u => u.IdNote, id);
            var update = Builders<NotesModel>.Update
            .Set(u => u.Title, model.Title)
            .Set(u => u.Contenido, model.Contenido)
            .Set(u => u.FechaUpdate, DateTime.Now);
            var response = await _database.UpdateOneAsync(filter, update);

            var fil = await ViewOne(id);
            var res = await UpdateDate(fil.IdLibreta);
            // UpdateDate(fil.IdLibreta
            return response.IsAcknowledged && response.ModifiedCount > 0;
        }

        public async Task<List<NotesModel>> ViewAllDataIdUser(string IdLibreta, int pagina)
        {
            var cantidad = 20;
            try
            {
                var filter = Builders<NotesModel>.Filter.Eq(note => note.IdLibreta, IdLibreta);
                return await _database.Find(filter)
                .SortByDescending(n => n.FechaUpdate)
                .Skip((pagina - 1) * cantidad)
                .Limit(cantidad)
                .ToListAsync();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<NotesModel> ViewOne(string Dato)
        {
            try
            {
                var note = await _database.Find(new BsonDocument { { "_id", new ObjectId(Dato) } }).FirstOrDefaultAsync();
                if (note == null) return null;
                return note;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteByIdUser(string id)
        {
            var delete = Builders<NotesModel>.Filter.Eq(x => x.IdLibreta, id);
            var response = await _database.DeleteManyAsync(delete);
            if (response.IsAcknowledged && response.DeletedCount > 0) return response.IsAcknowledged;
            return false;
        }

        public async Task<bool> UpdateDate(string id)
        {
            var filter = Builders<LibreriasModel>.Filter.Eq(e => e.IdLibreta, id);
            var update = Builders<LibreriasModel>.Update.Set(u => u.UpdateBook, DateTime.Now);
            var response = await _collection.UpdateOneAsync(filter, update);
            return response.IsAcknowledged;
        }
    }
}