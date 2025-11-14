using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Usuarios;
using BackEndNotes.Interfaces;
using BackEndNotes.Interfaces.Principals;
using BackEndNotes.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace BackEndNotes.Services
{
    public class UsuarioService
    {
        private readonly IUsuario<UpdateUserDto, UsuarioDataDto> _colection;
        public UsuarioService(IUsuario<UpdateUserDto, UsuarioDataDto> collection)
        {
            _colection = collection;
        }

        public async Task<UsuarioDataDto> VerDataUsuario(string id)
        {
            return await _colection.ViewOne(id);
        }

        public async Task<IEnumerable<UsuarioDataDto>> VerUsuarioEmail(string email)
        {
            return await _colection.ViewUserEmail(email);
        }

        public async Task<bool> ActualizarUsuario(string id, UpdateUserDto model)
        {
            return await _colection.UpdateData(id, model);
        }

        public async Task<bool> EliminarUsuario(string id)
        {
            return await _colection.Remove(id);
        }
    }
}