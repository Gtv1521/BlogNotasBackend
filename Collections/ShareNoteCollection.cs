using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BackEndNotes.Dto.Usuarios;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models;
using BackEndNotes.Models.Librerias;
using BackEndNotes.Models.Notes;
using BackEndNotes.Utils;
using BlogNotasBackend.Models;
using DnsClient.Protocol;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Search;
using src.Interfaces.Principals;
using src.Models.projeccions;

namespace src.Collections
{
    public class ShareNoteCollection : IShare<ShareNoteModel, ShareNoteProjection>
    {
        private readonly IMongoCollection<ShareNoteModel> _collection;
        private readonly IMongoCollection<NotesModel> _notes;
        private readonly IMongoCollection<UserModel> _users;
        private const int page = 20;
        public ShareNoteCollection(Context context)
        {
            _collection = context.GetCollection<ShareNoteModel>("Share");
            _notes = context.GetCollection<NotesModel>("Notas");
            _users = context.GetCollection<UserModel>("Usuarios");
        }

        public async Task<long> Count(string id)
        {
            var filter = Builders<ShareNoteModel>.Filter.Eq(x => x.IdUser, id);
            return await _collection.Find(filter).CountDocumentsAsync();
        }

        public async Task<string> Create(ShareNoteModel Object)
        {
            await _collection.InsertOneAsync(Object);
            return Object.Id.ToString();
        }

        public async Task<IEnumerable<ShareNoteProjection>> Filter(string filter, string id)
        {
            var text = new BsonRegularExpression(filter, "i");

            return await _collection.Aggregate()
            .Match(x => x.NoteId == id)
            .Lookup(
                foreignCollection: _notes,
                foreignField: x => x.IdNote,
                localField: x => x.NoteId,
                @as: (ShareNoteProjection x) => x.NoteDetails)
            .Lookup(
                foreignCollection: _users,
                foreignField: x => x.Id,
                localField: x => x.IdUserReference,
                @as: (ShareNoteProjection x) => x.UserDetails)
            .Match(Builders<ShareNoteProjection>.Filter.ElemMatch(
                x => x.NoteDetails,
                Builders<NotesModel>.Filter.Regex(x => x.Title, text)))
            .Project(x => new ShareNoteProjection
            {
                Id = x.Id,
                NoteId = x.NoteId,
                IdUser = x.IdUser,
                IdLibreta = x.IdLibreta,
                IdUserReference = x.IdUserReference,
                ReadPermits = x.ReadPermits,
                WritePermits = x.WritePermits,
                NoteDetails = x.NoteDetails,
                UserDetails = x.UserDetails.Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Password = null,
                    Role = u.Role
                }).ToList()
            })
            .ToListAsync();
        }

        public async Task<bool> Remove(string id)
        {
            var filter = Builders<ShareNoteModel>.Filter.Eq(x => x.Id, id);
            var get = _collection.Find(filter);

            if (!get.Any()) return false;
            var delete = await _collection.DeleteOneAsync(filter);
            if (delete.DeletedCount == 0) return false;
            return true;
        }

        // update permits 
        public async Task<bool> UpdateData(string id, ShareNoteModel model)
        {
            var filter = Builders<ShareNoteModel>.Filter.Eq(x => x.Id, id);
            var update = Builders<ShareNoteModel>.Update
            .Set(x => x.ReadPermits, model.ReadPermits)
            .Set(x => x.WritePermits, model.WritePermits);

            var response = await _collection.UpdateOneAsync(filter, update);
            if (response.ModifiedCount == 0) return false;
            return true;
        }

        public Task<bool> updatePermits(ShareNoteModel data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShareNoteProjection>> ViewAllDataIdUser(string Id, int pagina)
        {
            // id de usuario
            return await _collection.Aggregate()
            .Match(x => x.NoteId == Id)
            .Lookup(
                 foreignCollection: _notes,
                 foreignField: x => x.IdNote,
                 localField: x => x.NoteId,
                 @as: (ShareNoteProjection x) => x.NoteDetails
             )
            .Lookup(
                    foreignCollection: _users,
                    foreignField: x => x.Id,
                    localField: x => x.IdUserReference,
                    @as: (ShareNoteProjection x) => x.UserDetails
             )
            .Project(x => new ShareNoteProjection
            {
                Id = x.Id,
                NoteId = x.NoteId,
                IdUser = x.IdUser,
                IdLibreta = x.IdLibreta,
                IdUserReference = x.IdUserReference,
                ReadPermits = x.ReadPermits,
                WritePermits = x.WritePermits,
                NoteDetails = x.NoteDetails,
                UserDetails = x.UserDetails.Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Password = null,
                    Role = u.Role
                }).ToList()
            })
            .Skip((pagina - 1) * page)
            .Limit(page)
            .ToListAsync();
        }

        public async Task<ShareNoteModel> ViewOne(string Dato)
        {
            var response = Builders<ShareNoteModel>.Filter.Eq(x => x.Id,Dato);
            return await _collection.Find(response).FirstOrDefaultAsync();
        }

        // Entra el id de usuario referido y muestra las notas compartidas
        public async Task<IEnumerable<ShareNoteProjection>> GetShares(string Id)
        {
            System.Console.WriteLine(Id);
            return await _collection.Aggregate()
            .Match(x => x.IdUserReference == Id)
            .Lookup(
                foreignCollection: _notes,
                foreignField: x => x.IdNote,
                localField: x => x.NoteId,
                @as: (ShareNoteProjection x) => x.NoteDetails
            )
            .ToListAsync();
        }

        //  cambia el id  de string a objectid
        private ObjectId Change(string id)
        {
            return new ObjectId(id);
        }
    }
}