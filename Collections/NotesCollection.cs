using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces;
using BackEndNotes.Models.Notes;
using BackEndNotes.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace BackEndNotes.Collections
{
    public class NotesCollection : INotes<NotesModel, UpdateNoteDto>
    {
        private readonly IMongoCollection<NotesModel> _database;
        public NotesCollection(Context context)
        {
            _database = context.GetCollection<NotesModel>("Notas");
        }
        public async Task<string> Create(NotesModel Object)
        {
            try
            {
                await _database.InsertOneAsync(Object);
                return Object.IdNote;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<bool> Remove(string id)
        {
             var filter = Builders<NotesModel>.Filter.Eq(note => note.IdNote, id);
             var result = await _database.DeleteOneAsync(filter);
             if (result.IsAcknowledged && result.DeletedCount > 0) return result.IsAcknowledged;
             return false;
        }

        public Task<bool> Update(UpdateNoteDto model)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotesModel>> ViewAllDataIdUser(string userId)
        {
            try
            {
                var filter = Builders<NotesModel>.Filter.Eq(note => note.IdUser, userId);
                return _database.FindAsync(filter).Result.ToListAsync();
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


    }
}