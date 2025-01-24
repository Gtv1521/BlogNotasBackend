using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Services
{
    public class UserService
    {
        private readonly IViews<UserModel> _collection;

        public UserService(IViews<UserModel> collection)
        {
            _collection = collection;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _collection.ViewsAll();
        }

        public async Task<UserModel> GetUserById(string id)
        {
            return await _collection.ViewsById(id);
        }

        public async Task<bool> SignIn(UserDto user)
        {
            var NewUser = new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };

            return await _collection.Create(NewUser);
        }

    }
}