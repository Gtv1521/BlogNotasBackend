using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Services
{
    public class MailService
    {
        private readonly IViewOne _collection; 
        public MailService(IViewOne collection)
        {
            _collection = collection;
        }

        public async Task<bool> ValidaMail(string email)
        {
            return await _collection.ViewOne(email);
        }
    }
}