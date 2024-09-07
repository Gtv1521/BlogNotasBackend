using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Notas_Back.Dto;

namespace Notas_Back.Repositories
{
    public class generaToken
    {
        private readonly IConfiguration _configuration;
        public generaToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string crearToken(string NameUser)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, NameUser),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);
            var Pass = new JwtSecurityTokenHandler().WriteToken(token);
            return Pass.ToString();
        }
    }
}