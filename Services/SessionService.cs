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
        private readonly Token _token;

        public SessionService(ISessionUser<UserModel, PasswordDto, MailModel> service, Token token, MailCollection mail, ManejoPasswords password)
        {
            _service = service;
            _password = password;
            _token = token;
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
        public async Task<bool> ChangePassword(string IdUser, PasswordDto model)
        {
            return await _service.ChoosePassword(IdUser, model);
        }

        public async Task<bool> RestartPassword(string mail)
        {
            try
            {
                var email = await _mail.ViewOne(mail);
                if (email == null) return false;

                var token = _token.GenerateToken(email.Id.ToString(), 1);
                _service.Notificar(new MailModel
                {
                    Mail = mail,
                    Motivo = "Recuperación de contraseña",
                    Message = $"<h1>Hola {email.Name},</h1> <article>Para hacer cambio de su contraseña, haga click en el siguiente enlace:</article> http://localhost:5000/api/usuarios/resetPassword?id={email.Id}?token={token}"
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