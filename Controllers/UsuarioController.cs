using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Dto.Usuarios;
using BackEndNotes.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace BackEndNotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        // private readonly ILogger _log; 
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            // _log = log;
            _service = service;
        }


        /// <summary>
        /// Ver datos de usuario 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     GET
        ///     {
        ///         "id": "673d0d7ab2310184458dbfde"
        ///     }
        /// </remarks>
        /// <response code="200">usuarios</response>
        /// <response code="400">No hay datos</response>
        /// <response code="500">Error server.</response>

        [HttpGet]
        [Route("user/{id}")]
        [Consumes("application/json", "multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioDataDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> VerDataUsuario(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return BadRequest("Debe enviar el id del usuario");
                var user = await _service.VerDataUsuario(id);
                if (user == null) return NotFound(new ResponseDto { Message = "Id del usuario no se encontro " });
                return Ok(user);
            }
            catch (Exception ex)
            {
                // _log.LogError($"Error al obtener datos de usuario: {ex.Message}");
                return Problem($"Hubo un error inesperado: {ex.Message}", $"/api/usuario/user/{id}", 500, "Server error");

            }
        }

        /// <summary>
        /// muestre el usuario por el email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> VerUserEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest(new ResponseDto { Message = "Debe enviar un email" });
            var response = await _service.VerUsuarioEmail(email);
            if (response == null) return NotFound(new ResponseDto { Message = "usuario de este email no existe" });
            return Ok(response);
        }

        /// <summary>
        /// Actualiza datos del usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("update_user/{id}")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ActualizarUsuario(string id, [FromForm] UpdateUserDto user)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return BadRequest("Debe enviar el id del usuario");
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _service.ActualizarUsuario(id, user);

                if (!result) return BadRequest(new ResponseDto
                {
                    Message = "No se pudo actualizar el usuario"
                });

                return Ok(new ResponseDto
                {
                    Message = "Usuario actualizado correctamente"
                });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "api/Usuarios/update_user", 500, "Error server");
            }
        }

        [HttpDelete]
        [Route("delete_user/{id}")]
        [Consumes("application/json", "multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return BadRequest("Debe enviar el id del usuario");
                var result = await _service.EliminarUsuario(id);

                if (!result) return NotFound(new ResponseDto
                {
                    Message = "No se encontro el id del usuario"
                });

                return Ok(new ResponseDto
                {
                    Message = "Usuario eliminado correctamente"
                });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "api/usuarios/delete_user", 500, "Error server");
            }
        }
    }
}