using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using Notas_Back.Dto;
using Notas_Back.Interfaces;
using Notas_Back.Models;
using Notas_Back.Repositories;

namespace Notas_Back.Services
{
    public class UsuariosService : ICrud<Usuarios>
    {
        internal Context _context = new Context();
        public IMongoCollection<Usuarios> collection;
        private readonly ManejoContraseñas _manejoContraseñas = new ManejoContraseñas();

        public UsuariosService() // Constructor de la clase
        {
            collection = _context.db.GetCollection<Usuarios>("Usuarios");
        }

        // Ingresa un usuario nuevo y hace hass de la contraseña 
        public async Task Post(Usuarios modelo)
        {
            try
            {
                if (modelo.Password != null)
                {

                    string contraseña = _manejoContraseñas.HashearContraseña(modelo.Password.ToString());
                    Usuarios User = new Usuarios
                    {
                        Email = modelo.Email,
                        FirsName = modelo.FirsName,
                        LastName = modelo.LastName,
                        UserName = modelo.UserName,
                        Password = contraseña,
                        Rol = modelo.Rol
                    };

                    await collection.InsertOneAsync(User);
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
        }

        // Muestra un usuario por el id
        public async Task<Usuarios> Get(string IdUser)
        {
            return await collection.FindAsync(new BsonDocument { { "_id", new ObjectId(IdUser) } }).Result.FirstOrDefaultAsync();
        }

        // Muestra todos los usuarios
        public async Task<List<Usuarios>> GetAllUsers()
        {
            return await collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        // Actualiza datos de un usuario 
        public async Task Update(Usuarios modelo)
        {
            var filter = Builders<Usuarios>.Filter.Eq(s => s.Id, modelo.Id);
            await collection.ReplaceOneAsync(filter, modelo);
        }

        // Elimina un usuario 
        public async Task Delete(string id)
        {
            var filter = Builders<Usuarios>.Filter.Eq(s => s.Id, id);
            await collection.DeleteOneAsync(filter);
        }

        // cambiar contraseña 
        public async Task cambiarPass(string id, string pass)
        {
            string contraseña = _manejoContraseñas.HashearContraseña(pass);

            var filter = Builders<Usuarios>.Filter.Eq(u => u.Id, id);
            var password = Builders<Usuarios>.Update.Set(u => u.Password, contraseña);
            await collection.UpdateOneAsync(filter, password);
        }

        public async Task<Usuarios> verEmail(string email)
        {
            return await collection.FindAsync(new BsonDocument { { "Email", email } }).Result.FirstOrDefaultAsync();
        }

        public async Task<Usuarios> Init(string user, string pass)
        {
            try
            {
                var filter = Builders<Usuarios>.Filter.Eq(u => u.UserName, user);
                return await collection.FindAsync(filter).Result.FirstOrDefaultAsync();

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
        }
    }
}