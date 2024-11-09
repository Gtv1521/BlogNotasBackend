using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Bson;
using Notas_Back.Dto;
using NotasBack.Dto.entrance;
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
        /// <response code="200">Todos los usuarios.</response>
        /// <response code="400">No se encuentran datos</response>
        /// <response code="401">No Autorizado</response>
        [HttpGet]
        [Route("VerUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UsuariosM>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(NoData))]
        // [Authorize]
        public async Task<IActionResult> Users()
        {
            try
            {
                return Ok(await _service.GetAllUsers());
            }
            catch (System.Exception)
            {
                return BadRequest(new NoData
                {
                    status = 400,
                    mensaje = "Error al cargar los datos"
                });
            }
        }


        /// <summary>
        /// Muestra los datos de un usuario por el id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true</returns>
        /// <remarks>
        /// Sample request:  
        ///     
        ///     GET /api/USer/{id}
        ///     {
        ///         "Id" = "66dd00816d2edc4b82609d8c",
        ///     }
        /// </remarks>
        /// <response code="200">Usuario</response>
        /// <response code="400">Usuario no encontrado</response>
        /// <response code="401">No Autorizado</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuariosM))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(NoData))]
        [Authorize]
        public async Task<IActionResult> MostrarUser(string Id)
        {
            try
            {
                return Ok(await _service.Get(Id));
            }
            catch (System.Exception)
            {

                return BadRequest(new NoData
                {
                    status = 404,
                    mensaje = "Error al cargar los datos"
                });
            }
        }

        /// <summary>
        /// Muestra si el email esta agregado a la base
        /// </summary>
        /// <param name="email"></param>
        /// <remarks>
        /// Sample request:  
        ///     
        ///     GET /api/USer/Email
        ///     {
        ///         "Email" = "gustavober98@gmail.com",
        ///     }
        /// </remarks>
        /// <response code="200">Response.</response>
        /// <response code="400">Usuario no encontrado.</response>
        /// <response code="401">No Autorizado</response>
        [HttpGet]
        // [Authorize]
        [Route("Email")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuariosM))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(NoData))]
        public async Task<IActionResult> verEmail(string email)
        {
            if (email != null)
            {
                UsuariosM result = await _service.verEmail(email);
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
                    status = 400,
                    mensaje = "Debe enviar un email"
                });
            }
        }


        /// <summary>
        /// Actualiza datos de un usuario 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns>true</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/User/updateUser
        ///     {
        ///         "id": "66dd00816d2edc4b82609d8c",
        ///         "email": "gustavober98@gmail.com",
        ///         "firsName": "Gustavo",
        ///         "lastName": "Bernal",
        ///         "userName": "Gus123"
        ///     }
        /// </remarks>
        /// <response code="201">Update</response>
        /// <response code="400">Algo fallo</response>
        /// <response code="404">No encontrado</response>
        [HttpPut]
        [Authorize]
        [Route("updateUser")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NoData))]
        public async Task<IActionResult> putUser([FromBody] UsuariosM user, string id)
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

                if (user.Email == string.Empty)
                {
                    ModelState.AddModelError("Email", "The LastName shouldn´t be Empty");
                }

                user.Id = id;
                await _service.Update(user);
                return Created("Created", new NoData
                {
                    status = 201,
                    mensaje = "Datos actualizados"
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest("No se pudo crear el usuario");
                throw new ApplicationException($"Hubo un error {ex.Message}");
            }
        }

        /// <summary>
        ///  Elimina un usuario de la aplicación
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns>true</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     DELETE /api/User/Delete
        ///     {
        ///         "IdUser" : "66dd00816d2edc4b82609d8c"
        ///     }
        /// </remarks>
        /// <response code="200">Eliminado.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="404">No se encontro usuario.</response>
        [HttpDelete]
        [Route("deleteUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(NoData))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NoData))]
        public async Task<IActionResult> DeleteUser(string IdUser)
        {
            try
            {
                await _service.Delete(IdUser);
                return Ok(new NoData { status = 204, mensaje = "Delete user successfully" });
            }
            catch (System.Exception ex)
            {
                return NotFound(new NoData{ status = 404, mensaje = "No data found" });
                throw new ApplicationException($"Fallo deleted {ex.Message}");
            }
        }
    }
}