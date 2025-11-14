using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Interfaces.Regulars;

namespace src.Utils
{
    public class MemoryToken : ITokenBlacklist
    {
        private readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

        public bool IsRevoked(string jti)
        {
            var now = DateTime.UtcNow;
            foreach (var token in _revokedTokens.Where(t => t.Value < now).ToList())
            {
                _revokedTokens.TryRemove(token.Key, out _);
            }

            return _revokedTokens.ContainsKey(jti);
        }

        public void Revoke(string jti, DateTime expiration)
        {
            _revokedTokens.TryAdd(jti, expiration);
        }
    }
}