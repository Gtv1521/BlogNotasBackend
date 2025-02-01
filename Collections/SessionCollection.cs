using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models;
using BackEndNotes.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackEndNotes.Collections
{
    public class SessionCollection : ISessionUser<UserModel, PasswordDto, MailModel>
    {
        private readonly IMongoCollection<UserModel> _collection;
        private readonly MailCollection _mail;
        private readonly NotificationMail _sendMail;
        private readonly ManejoPasswords _password;

        public SessionCollection(Context context, ManejoPasswords passwords, MailCollection mail, NotificationMail sendNail)
        {
            _collection = context.GetCollection<UserModel>("Usuarios");
            _password = passwords;
            _mail = mail;
            _sendMail = sendNail;
        }

        public async Task<bool> ChoosePassword(PasswordDto model)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, model.IdUser);
            var update = Builders<UserModel>.Update.Set(u => u.Password, _password.HashearContraseña(model.Password));
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<string> Create(UserModel Object)
        {
            try
            {
                await _collection.InsertOneAsync(Object);
                return Object.Id;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        // Metodo para iniciar session de usuarios 
        public async Task<UserModel> Login(string email, string pass)
        {

            var mail = await _mail.ViewOne(email);
            if (mail?.Password == null) return null;

            if (_password.VerifyContraseña(pass, mail.Password))
            {
                return mail;
            }
            else
            {
                return null;
            }
        }

        public bool Notificar(MailModel notification)
        {
            try
            {
                _sendMail.Notificar(notification);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}