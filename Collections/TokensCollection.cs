using System;
using System.Collections.Generic;
using System.Linq;
using BlogNotasBackend.Interfaces.Principals;
using BlogNotasBackend.Models;
using BackEndNotes.Utils;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

namespace BlogNotasBackend.Collections
{
    public class TokensCollection : ITokens
    {

        private readonly IMongoCollection<SessionModel> _session;

        public TokensCollection(Context context)
        {
            _session = context.GetCollection<SessionModel>("Session");

        }

        // valida que el token se sesion exista
        public async Task<string> Refresh(string TokenRefresh)
        {
            var filter = Builders<SessionModel>.Filter.Eq(x => x.TokenRefresh, TokenRefresh);
            var response = await _session.Find(filter).FirstOrDefaultAsync();
            if (response == null) return null;
            return response.IdUser;
        }


        //  crea token de refresh y lo agrega a la db
        public async Task<string> CreatedTokenRefresh(string IdUser)
        {
            var token = Guid.NewGuid().ToString(); // crea nuevo token 
            var insert = new SessionModel
            {
                IdUser = IdUser,
                TokenRefresh = token
            };

            await _session.InsertOneAsync(insert);
            if (insert.IdSession != null) return token;
            return null;
        }

        //  Elimina tokenRefresh y acaba session de usuario
        public async Task<bool> Logout(string token)
        {
            var filter = Builders<SessionModel>.Filter.Eq(x => x.TokenRefresh, token);
            var response = await _session.DeleteOneAsync(filter);

            if (response.IsAcknowledged && response.DeletedCount > 0) return response.IsAcknowledged;
            return false;
        }
    }
}