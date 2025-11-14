using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Interfaces.Regulars
{
    public interface ITokenBlacklist
    {
        void Revoke(string jti, DateTime expiration);
        bool IsRevoked(string jti);
    }

}