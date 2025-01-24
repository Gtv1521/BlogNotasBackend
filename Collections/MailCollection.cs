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
    public class MailCollection : IViewOne
    {
        private readonly IMongoCollection<UserModel> _database;
        public MailCollection(Context context)
        {
            _database = context.GetCollection<UserModel>("Usuarios");
        }

        public async Task<bool> ViewOne(string Dato)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Email, Dato);
            var Mail =  await _database.Find(filter).FirstOrDefaultAsync();
            if (Mail == null) return false;
            return true;
        }
    }
}