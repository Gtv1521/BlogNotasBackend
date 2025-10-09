using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogNotasBackend.Interfaces.Principals;

namespace src.Services
{
    public class TokenService
    {
        private readonly ITokens _token;

        public TokenService(ITokens token)
        {
            _token = token;
        }

        // crea un token de refresh
        public async Task<string> CreateToken(string idUser)
        {
            return await _token.CreatedTokenRefresh(idUser);
        }

        // refresca el token de acceso
        public async Task<string> RefreshToken(string token)
        {
            return await _token.Refresh(token);
        }

        // elimina token de refresh 
        public async Task<bool> DeleteTokenRefresh(string token)
        {
            return await _token.Logout(token);
        }

    }
}