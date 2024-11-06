using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Notas_Back.Dto;
using Notas_Back.Models;
using Notas_Back.Repositories;
using Notas_Back.Services;

namespace Notas_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Session : ControllerBase
    {
        private readonly UsuariosService _UsuariosService;
        private readonly generaToken _token;
        private readonly ManejoContraseñas _manejoContraseñas;
        private readonly EnviarCorreo _enviarCorreo;

        public Session(UsuariosService usuariosService, generaToken token, ManejoContraseñas manejoContraseñas, EnviarCorreo correo)
        {
            _UsuariosService = usuariosService;
            _manejoContraseñas = manejoContraseñas;
            _enviarCorreo = correo;
            _token = token;
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns>true</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SigIn
        ///     {
        ///         "email": "gustavober98@gmail.com",
        ///         "firsName": "Gustavo",
        ///         "lastName": "Bernal Acero",
        ///         "userName": "Gustavober98",
        ///         "password": "123",
        ///         "rol": "Administrador"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">{
        ///           "status": 201,
        ///            "mensaje": "Usuario agregado con éxito"
        /// }</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("SigIn")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewUser([FromBody] UsuariosM usuarios)
        {
            try
            {
                if (usuarios == null) return BadRequest();

                if (usuarios.LastName == string.Empty)
                {
                    ModelState.AddModelError("LastName", "The LastName shouldn´t be Empty");
                }

                if (usuarios.FirsName == string.Empty)
                {
                    ModelState.AddModelError("FirstName", "The FirsName shouldn´t be Empty");
                }

                if (usuarios.Password == string.Empty)
                {
                    ModelState.AddModelError("Password", "The Password shouldn´t be Empty");
                }

                if (usuarios.Email == string.Empty)
                {
                    ModelState.AddModelError("Email", "The Email shouldn´t be Empty");
                }

                UsuariosM Mail = await _UsuariosService.verEmail(usuarios.Email.ToString());
                UsuariosM User = await _UsuariosService.verUserName(usuarios.UserName.ToString());
                if (Mail != null)
                {
                    return StatusCode(409, new NoData
                    {
                        status = 409,
                        mensaje = "Email ya existe"
                    });
                }
                if (User != null)
                {
                    return StatusCode(409, new NoData
                    {
                        status = 409,
                        mensaje = "UserName ya existe"
                    });
                }

                var Mesagge = "<h1>Bienvenido a MyBloggg</H1>" +
                "<h4>Te damos un recibimiento esperamos te guste esta esperiencia</h4>" +
                "Tu usuario: <b>" + usuarios.UserName + "</b>" +
                 "<p>Esta App esta construida para aquellos que lo secuestra la inspiracion en cualquier momento</p>" +
                 "<p>Nos complace que seas parte de esta gran experiencia y que te animes a probar las cosas nuevas que ofrece la tecnologia.</p>";


                await _UsuariosService.Post(usuarios);
                bool result = _enviarCorreo.EnviarCorreoNotificacion(usuarios.Email.ToString(), Mesagge);
                string token = _token.crearToken(usuarios.UserName.ToString());

                return Created("created", new
                {
                    status = 201,
                    mensaje = "Usuario agregado con éxito",
                    token
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest("No se pudo crear el usuario" + ex);
                throw;
            }
        }

        /// <summary>
        /// Devuelve los datos de inicio de sesión
        /// </summary>
        /// <param name="NameUser"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LogIn")]
        public async Task<IActionResult> Inicio(string NameUser, string Password)
        {
            if (NameUser != null && Password != null)
            {
                UsuariosM respuesta = await _UsuariosService.Init(NameUser, Password);
                if (respuesta?.Password != null)
                {
                    bool statusPass = _manejoContraseñas.VerifyContraseña(Password, respuesta.Password.ToString());
                    if (statusPass)
                    {
                        string token = _token.crearToken(NameUser);
                        return Ok(new
                        {
                            Id = respuesta.Id,
                            UserName = respuesta.UserName,
                            Nombre = respuesta.FirsName,
                            Apellido = respuesta.LastName,
                            Email = respuesta.Email,
                            Mensaje = "Bienvenido A Blog Notas",
                            token
                        });
                    }
                    else
                    {
                        return Unauthorized(new NoData
                        {
                            status = 401,
                            mensaje = "contraseña incorrecta"
                        });
                    }
                }
                else
                {
                    return Unauthorized(new NoData
                    {
                        status = 401,
                        mensaje = "Usuario ó Contraseña incorrectos"
                    });

                }
            }
            else
            {
                return BadRequest(new NoData
                {
                    status = 404,
                    mensaje = "Enviar datos completos"
                });
            }
        }


        /// <summary>
        /// Actualiza un usuario 
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="pass2"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatePassword/{id}")]
        public async Task<IActionResult> putPass(string pass, string pass2, string id)
        {
            if (pass == pass2)
            {
                await _UsuariosService.cambiarPass(id, pass);
                return Created("created", new NoData
                {
                    status = 201,
                    mensaje = "Datos actualizados"
                });
            }
            else
            {
                return BadRequest("Contraseñas no son iguales");
            }
        }

    }
}