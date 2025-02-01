using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Collections;
using BackEndNotes.Dto;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models;
using BackEndNotes.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Services
{
    public class SessionService
    {
        private readonly ISessionUser<UserModel, PasswordDto, MailModel> _service;
        private readonly ManejoPasswords _password;
        private readonly MailCollection _mail;

        public SessionService(ISessionUser<UserModel, PasswordDto, MailModel> service, MailCollection mail, ManejoPasswords password)
        {
            _service = service;
            _password = password;
            _mail = mail;
        }
        // Ingresa un usuario a la base 
        public async Task<string> SignIn(UserDto user)
        {

            return await _service.Create(new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Password = _password.HashearContraseña(user.Password),
                Role = user.Role
            });
        }

        // Inicia sesion con los datos de la base de datos
        public async Task<UserModel> Login(string email, string pass)
        {
            return await _service.Login(email, pass);
        }

        // cambia contraseña con el id de usuario 
        public async Task<bool> ChangePassword(PasswordDto model)
        {
            return await _service.ChoosePassword(model);
        }

        public async Task<bool> RestartPassword(string mail)
        {
            try
            {
                var email = await _mail.ViewOne(mail);
                if (email == null) return false;

                _service.Notificar(new MailModel
                {
                    Mail = mail,
                    Motivo = "Recuperación de contraseña",
                    Message = $"Hola {email.Name}, para recuperar su contraseña, haga click en el siguiente enlace: http://localhost:5000/api/usuarios/resetPassword/{email.Id}"
                });
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}