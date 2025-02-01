using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndNotes.Utils
{
    public class ManejoPasswords
    {
        // Encryption password
        public string HashearContraseña(string contraseña)
        {
            return BCrypt.Net.BCrypt.HashPassword(contraseña);
        }
        
        // Verify password 
        public bool VerifyContraseña(string contraseña, string hashAlmacenado)
        {
            return BCrypt.Net.BCrypt.Verify(contraseña, hashAlmacenado);
        }
    }
}