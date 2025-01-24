using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BackEndNotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly UserService _service;
        private readonly MailService _mailService;
        public SessionController(UserService service, MailService mailService)
        {
            _service = service;
            _mailService = mailService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <returns>returna un bool que verifica si existe o no el email</returns>
        [HttpGet]
        [Route("checkmail")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CheckMail([FromQuery] string email)
        {
            try
            {
                return Ok(await _mailService.ValidaMail(email));
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "/api/session/checkmail", 500, "Server error");
            }
        }


        // [HttpGet]
        // [Route("Login")]
        // public async Task<IActionResult> Login([FromQuery] string email, string password)
        // {
        //     try
        //     {
        //         return Ok(await _service.Login(email, password));
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return Problem(ex.Message, "/api/session/login", 500, "Server error");
        //     }
        // }

        // [HttpGet]
        // [Route("Logout")]
        // public async Task<IActionResult> Logout([FromQuery] string token)
        // {  
        //     try
        //     {
        //         return Ok(await _service.Logout(token));
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return Problem(ex.Message, "/api/session/logout", 500, "Server error");
        //     }
        // }

        /// <summary>
        /// Crea un nuevo usuario en el sistema 
        /// </summary>
        /// <param name="User"></param>
        /// <returns>Usuario creado</returns>
        /// 
        [HttpPost]
        [Route("SingIn")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequest))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> SingIn([FromForm] UserDto User)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // retorna la respuesta almacenada en el modelo 
                }

                if (await _service.SignIn(User))
                {
                    return Ok(new
                    {
                        Message = "Usuario creado correctamente"
                    });
                }
                return BadRequest(new {Messagge = "No se pudo crear usuario"});

            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "/api/session/singin", 500, "Server error");
            }
        }
    }
}