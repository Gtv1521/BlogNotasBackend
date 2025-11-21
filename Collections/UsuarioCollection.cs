using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Usuarios;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models;
using BackEndNotes.Utils;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackEndNotes.Collections
{
    public class UsuarioCollection : IUsuario<UpdateUserDto, UsuarioDataDto>
    {
        private readonly IMongoCollection<UserModel> _database;
        public UsuarioCollection(Context context)
        {
            _database = context.GetCollection<UserModel>("Usuarios");
        }

        public async Task<bool> Remove(string id)
        {
            var filter = Builders<UserModel>.Filter.Eq(user => user.Id, Change(id));
            var result = await _database.DeleteOneAsync(filter);
            if (result.IsAcknowledged && result.DeletedCount > 0) return result.IsAcknowledged;
            return false;
        }

        public async Task<bool> UpdateData(string id, UpdateUserDto model)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, Change(id));
            var update = Builders<UserModel>.Update
            .Set(u => u.Name, model.Name)
            .Set(u => u.Role, model.Role);
            var response = await _database.UpdateOneAsync(filter, update);
            return response.IsAcknowledged && response.ModifiedCount > 0; // Retorna un verdadero o falso 
        }

        public async Task<UsuarioDataDto> ViewOne(string Dato)
        {
            var result = await _database.FindAsync(new BsonDocument { { "_id", new ObjectId(Dato) } }).Result.FirstOrDefaultAsync();
            if (result == null) return null;
            return new UsuarioDataDto
            {
                IdUser = Dato,
                Name = result?.Name,
                Email = result?.Email,
                Role = result?.Role
            };
        }

        public async Task<IEnumerable<UsuarioDataDto>> ViewUserEmail(string email)
        {
            var filtro = Builders<UserModel>.Filter.Regex(x => x.Email, email);
            return await _database.Find(filtro)
                .Project(x => new UsuarioDataDto
                {
                    IdUser = x.Id.ToString(),
                    Name = x.Name,
                    Email = x.Email,
                    Role = x.Role
                })
                .ToListAsync();
        }

        private ObjectId Change(string id)
        {
            return new ObjectId(id);
        }
    }
}