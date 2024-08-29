using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Notas_Back.Dto;
using Notas_Back.Models;

namespace Notas_Back.Services
{
    public class ManejoContraseñas
    {
        public string HashearContraseña(string contraseña)
        {
            return BCrypt.Net.BCrypt.HashPassword(contraseña);
        }

        public bool VerifyContraseña(string contraseña, string hashAlmacenado)
        {
            return BCrypt.Net.BCrypt.Verify(contraseña, hashAlmacenado);
        }
    }
}