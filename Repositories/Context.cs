using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notas_Back.Models;

namespace Notas_Back.Repositories
{
    public class Context
    {
        private readonly IMongoDatabase _db;
        public Context(IOptions<MongoConections> settings)
        {
            try
            {
                var client = new MongoClient(settings.Value.ConnectionStrings);
                _db = client.GetDatabase(settings.Value.DatabaseName);
                
                System.Console.WriteLine("connect successfully");
            }
            catch (MongoException ex)
            {
                Console.WriteLine($"Error al conectar a MongoDB: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"No se pudo conectar a la base de datos {ex.Message}"); ;
            }
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }

}