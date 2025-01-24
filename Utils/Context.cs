using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BackEndNotes.Utils
{
    public class Context
    {
        private readonly IMongoDatabase _database;
        public Context(IOptions<DatabaseModel> settings)
        {
            try
            {
                if (settings.Value.Connection != null && settings.Value.Database != null)
                {
                    var client = new MongoClient(settings.Value.Connection);
                    _database = client.GetDatabase(settings.Value.Database);
                }
            }
            catch (MongoException ex)
            {
                System.Console.WriteLine("Error of connection" + ex.Message);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Error connecting to {ex.Message}");
            }
        }

        public IMongoCollection<T> GetCollection<T>(string Collection)
        {
            return _database.GetCollection<T>(Collection);
        }
    }
}