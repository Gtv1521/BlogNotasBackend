using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Services
{
    public class MailService
    {
        private readonly IViewOne<UserModel> _collection; 
        public MailService(IViewOne<UserModel> collection)
        {
            _collection = collection;
        }

        public async Task<bool> ValidaMail(string email)
        {
            if (await _collection.ViewOne(email) == null) return false;
            return true;
        }
    }
}