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
using Notas_Back.Services;

namespace Notas_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Session : ControllerBase
    {
        private readonly UsuariosService _UsuariosService;
        private readonly ManejoContraseñas _manejoContraseñas;

        private readonly IConfiguration _configuration;

        public Session(UsuariosService usuariosService, ManejoContraseñas manejoContraseñas, IConfiguration configuration)
        {
            _UsuariosService = usuariosService;
            _manejoContraseñas = manejoContraseñas;
            _configuration = configuration;
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
        public async Task<IActionResult> NewUser([FromBody] Usuarios usuarios)
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

                await _UsuariosService.Post(usuarios);
                return Created("created", new NoData
                {
                    status = 201,
                    mensaje = "Usuario agregado con éxito"
                });
            }
            catch (System.Exception)
            {
                return BadRequest("No se pudo crear el usuario");
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
                Usuarios respuesta = await _UsuariosService.Init(NameUser, Password);
                if (respuesta?.Password != null)
                {
                    bool statusPass = _manejoContraseñas.VerifyContraseña(Password, respuesta.Password.ToString());
                    if (statusPass)
                    {
                        var claims = new[]
                     {
                new Claim(JwtRegisteredClaimNames.Sub, NameUser),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Issuer"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: creds);

                        return Ok(new
                        {
                            Id = respuesta.Id,
                            UserName = respuesta.UserName,
                            Nombre = respuesta.FirsName,
                            Apellido = respuesta.LastName,
                            Email = respuesta.Email,
                            Mensaje = "Bienvenido A Blog Notas",
                            Token = new JwtSecurityTokenHandler().WriteToken(token)
                        });


                        // return Ok(respuesta);
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