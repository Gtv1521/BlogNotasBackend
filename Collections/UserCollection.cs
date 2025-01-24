using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using BackEndNotes.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackEndNotes.Collections
{
    public class UserCollection : IViews<UserModel>, ICreated<UserModel>
    {
        private readonly IMongoCollection<UserModel> _collection;

        public UserCollection(Context context)
        {
            _collection = context.GetCollection<UserModel>("Usuarios");
        }

        public async Task<bool> Create(UserModel Object)
        {
            try
            {
                await _collection.InsertOneAsync(Object);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserModel>> ViewsAll()
        {
            return await _collection.FindAsync(new BsonDocument { }).Result.ToListAsync();
        }

        public Task<UserModel> ViewsById(string id)
        {
            throw new NotImplementedException();
        }
    }
}