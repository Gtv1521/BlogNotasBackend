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
    public class UsuariosService : ICrud<UsuariosM>
    {
        public IMongoCollection<UsuariosM> collection;
        private readonly ManejoContraseñas _manejoContraseñas;

        public UsuariosService(Context context, ManejoContraseñas manejoContraseñas) // Constructor de la clase
        {
            _manejoContraseñas = manejoContraseñas;
            collection = context.GetCollection<UsuariosM>("Usuarios");
        }

        // Ingresa un usuario nuevo y hace hass de la contraseña 
        public async Task Post(UsuariosM modelo)
        {
            try
            {
                if (modelo.Password != null)
                {

                    string contraseña = _manejoContraseñas.HashearContraseña(modelo.Password.ToString());
                    UsuariosM User = new UsuariosM
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
                throw new ApplicationException($"no se pudo ingresar el usuario _{ex.Message}");
            }
        }

        // Muestra un usuario por el id
        public async Task<UsuariosM> Get(string IdUser)
        {
            return await collection.FindAsync(new BsonDocument { { "_id", new ObjectId(IdUser) } }).Result.FirstOrDefaultAsync();
        }

        // Muestra todos los usuarios
        public async Task<List<UsuariosM>> GetAllUsers()
        {
            return await collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        // Actualiza datos de un usuario 
        public async Task Update(UsuariosM modelo)
        {
            var filter = Builders<UsuariosM>.Filter.Eq(s => s.Id, modelo.Id);
            await collection.ReplaceOneAsync(filter, modelo);
        }

        // Elimina un usuario 
        public async Task Delete(string id)
        {
            var filter = Builders<UsuariosM>.Filter.Eq(s => s.Id, id);
            await collection.DeleteOneAsync(filter);
        }

        // cambiar contraseña 
        public async Task cambiarPass(string id, string pass)
        {
            string contraseña = _manejoContraseñas.HashearContraseña(pass);

            var filter = Builders<UsuariosM>.Filter.Eq(u => u.Id, id);
            var password = Builders<UsuariosM>.Update.Set(u => u.Password, contraseña);
            await collection.UpdateOneAsync(filter, password);
        }

        // verifica si el email existe
        public async Task<UsuariosM> verEmail(string email)
        {
            return await collection.FindAsync(new BsonDocument { { "Email", email } }).Result.FirstOrDefaultAsync();
        }

        // muestra un usuario por el id
        public async Task<UsuariosM> verUserName(string user)
        {
            try
            {
                var filter = Builders<UsuariosM>.Filter.Eq(u => u.UserName, user);
                return await collection.FindAsync(filter).Result.FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se pueden cargar usuario _{ex.Message}");

            }
        }


        // hace login en esta funcion
        public async Task<UsuariosM> Init(string user, string pass)
        {
            try
            {
                var filter = Builders<UsuariosM>.Filter.Eq(u => u.UserName, user);
                return await collection.FindAsync(filter).Result.FirstOrDefaultAsync();

            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se pueden cargar los datos _{ex.Message}");
            }
        }
    }
}