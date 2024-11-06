using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using MongoDB.Driver;
using Notas_Back.Interfaces;
using Notas_Back.Models;
using Notas_Back.Repositories;

namespace Notas_Back.Services
{
    public class NotasService : ICrud<Notas>
    {
        public IMongoCollection<Notas> collection;
        public NotasService(Context context)
        {
            collection = context.GetCollection<Notas>("Notas");
        }
        public async Task Delete(string Id)
        {
            try
            {
                var filter = Builders<Notas>.Filter.Eq(s => s.IdNota, Id);
                await collection.FindOneAndDeleteAsync(filter);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se Borrar el dato _{ex.Message}");
            }
        }

        // Muestra un listado de notas 
        public async Task<Notas> Get(string Id)
        {
            try
            {
                return await collection.FindAsync(new BsonDocument { { "_id", new ObjectId(Id) } }).Result.FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se pueden cargar los datos _{ex.Message}");
            }
        }

        // Muestra todas las notas del usuario
        public async Task<List<Notas>> allNotasByUser(string id)
        {
            try
            {
                return await collection.FindAsync(new BsonDocument { { "IdUser", id } }).Result.ToListAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se pueden cargar los datos _{ex.Message}");
            }
        }

        // Inserta una nota en la base 
        public async Task Post(Notas modelo)
        {
            try
            {
                if (modelo != null)
                {
                    await collection.InsertOneAsync(modelo);
                }
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se pueden cargar los datos _{ex.Message}");
            }
        }

        public async Task Update(Notas modelo)
        {
            try
            {
                var filter = Builders<Notas>.Filter.Eq(s => s.IdNota, modelo.IdNota);
                var update = Builders<Notas>.Update
                                        .Set("Titulo", modelo.Titulo)
                                        .Set("Contenido", modelo.Contenido)
                                        .Set("FechaUpdate", modelo.FechaUpdate);
                await collection.UpdateOneAsync(filter, update);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"no se pueden cargar los datos _{ex.Message}");
            }
        }
    }
}