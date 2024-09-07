using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Bson;
using Notas_Back.Dto;
using Notas_Back.Models;
using Notas_Back.Services;

namespace Notas_Back.Controllers
{
    /// <summary>
    /// Modulo para gestión de usuarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class User : ControllerBase
    {
        private readonly UsuariosService _service;

        public User(UsuariosService usuariosService)
        {
            _service = usuariosService;
        }

        /// <summary>
        ///  Muestra todos los  datos de los usuarios registrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VerUsuarios")]
        // [Authorize]
        public async Task<IActionResult> Users()
        {
            return Ok(await _service.GetAllUsers());
        }


        /// <summary>
        /// Muestra los datos de un usuario por el id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> MostrarUser(string id)
        {
            return Ok(await _service.Get(id));
        }

        /// <summary>
        /// Muestra si el email esta agregado a la base
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Email")]
        public async Task<IActionResult> verEmail(string email)
        {
            if (email != null)
            {
                Usuarios result = await _service.verEmail(email);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new NoData
                    {
                        status = 404,
                        mensaje = "Email no existe"
                    });
                }
            }
            else
            {
                return BadRequest(new NoData
                {
                    status = 404,
                    mensaje = "Debe enviar un email"
                });
            }
        }


        /// <summary>
        /// Actualiza datos de un usuario 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("updateUser")]
        public async Task<IActionResult> putUser([FromBody] Usuarios user, string id)
        {
            try
            {
                if (user == null) return BadRequest();

                if (user.LastName == string.Empty)
                {
                    ModelState.AddModelError("LastName", "The LastName shouldn´t be Empty");
                }

                if (user.FirsName == string.Empty)
                {
                    ModelState.AddModelError("FirstName", "The LastName shouldn´t be Empty");
                }

                if (user.Password == string.Empty)
                {
                    ModelState.AddModelError("Password", "The LastName shouldn´t be Empty");
                }

                if (user.Email == string.Empty)
                {
                    ModelState.AddModelError("Email", "The LastName shouldn´t be Empty");
                }

                user.Id = id;
                await _service.Update(user);
                return Created("Created", true);
            }
            catch (System.Exception)
            {
                return BadRequest("No se pudo crear el usuario");
                throw;
            }
        }

        /// <summary>
        ///  Elimina un usuario de la aplicación
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteUser")]
        public async Task<IActionResult> DeleteUser(string IdUser)
        {
            await _service.Delete(IdUser);
            return NoContent();
        }
    }
}