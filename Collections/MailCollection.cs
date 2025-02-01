using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using BackEndNotes.Utils;
using MongoDB.Driver;

namespace BackEndNotes.Collections
{
    public class MailCollection : IViewOne<UserModel>
    {
        private readonly IMongoCollection<UserModel> _database;
        public MailCollection(Context context)
        {
            _database = context.GetCollection<UserModel>("Usuarios");
        }

        public async Task<UserModel> ViewOne(string Dato)
        {   
            var filter = Builders<UserModel>.Filter.Eq(u => u.Email, Dato);
            return await _database.Find(filter).FirstOrDefaultAsync();
        }
    }
}