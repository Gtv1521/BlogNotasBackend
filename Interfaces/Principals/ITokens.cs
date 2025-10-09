using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNotasBackend.Interfaces.Principals
{
    public interface ITokens
    {
        Task<string> Refresh(string TokenRefresh); // retorna el idUser
        Task<string> CreatedTokenRefresh(string idUser);
        Task<bool> Logout(string TokenRefresh);
    }
}