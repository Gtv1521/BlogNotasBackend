using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using MongoDB.Driver;

namespace Notas_Back.Repositories
{
    public class Context
    {
        public MongoClient? client;
        public IMongoDatabase? db;
        public Context()
        {
            var env = DotEnv.Read();
            client = new MongoClient(env["Database"]);
            db = client.GetDatabase("BlogNotas");
        }
    }
}