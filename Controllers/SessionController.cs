using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using BackEndNotes.Services;
using BackEndNotes.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using src.Interfaces.Regulars;
using src.Services;

namespace BackEndNotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly Token _token;
        private readonly TokenService _serviceToken;
        private readonly SessionService _service;
        private readonly MailService _mailService;
        private readonly ITokenBlacklist _blackList;
        private readonly INotification<MailModel> _notificationMail;

        public SessionController(ILogger<SessionController> logger, ITokenBlacklist blackList, SessionService service, MailService mailService, INotification<MailModel> notificationMail, Token token, TokenService serviceToken)
        {
            _service = service;
            _serviceToken = serviceToken;
            _mailService = mailService;
            _notificationMail = notificationMail;
            _token = token;
            _blackList = blackList;
            _logger = logger;
        }

        /// <summary>
        /// Valida si el email existe en la base de datos
        /// </summary>
        /// <param name="email"></param>
        /// <returns>retorna un bool que verifica si existe o no el email</returns>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET 
        ///     {
        ///         "email": "gustavober98@gmail.com"
        ///     }
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="404">No encontrado</response>
        /// <response code="500">Error server</response>
        [HttpGet]
        [Route("check_mail/{email}")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CheckMail(string email)
        {
            try
            {
                if (email == null) return BadRequest(new ResponseDto { Message = "Ingrese un email." });
                if (await _mailService.ValidaMail(email)) return Ok(true);
                return BadRequest(false);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "/api/session/checkmail", 500, "Server error");
            }
        }

        /// <summary>
        /// Reset password in email "olvide la contraseña"
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     GET 
        ///     {
        ///         "email": "gustavober98@gmail.com"
        ///     }
        /// </remarks>
        /// <response code="200"></response> 
        /// <response code="400"></response> 
        /// <response code="500"></response> 
        [HttpGet]
        [Route("reset_password/{email}")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (await _service.RestartPassword(email)) return Ok(new ResponseDto { Message = "Verifica el correo y sigue los pasos..." });
                return BadRequest(new ResponseDto { Message = "Email not found" });
            }
            catch (System.Exception)
            {
                return Problem("Hubo un error inesperado", "/api/session/reset_password", 500, "Server error");
            }
        }


        /// <summary>
        /// Inicia session 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET
        ///     {
        ///         "Email": "user@example.com",
        ///         "Password": "stringst"
        ///     } 
        /// </remarks>
        /// <param name="login"></param>
        /// <returns>Responde con el usuario y el token de acceso</returns>
        /// <response code="200">Usuario</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("log_in")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Login([FromForm] LoginDto login)
        {
            try
            {
                if (!ModelState.IsValid)
                { }
                var user = await _service.Login(login.Email, login.Password);

                if (user == null) return NotFound(new ResponseDto { Message = "Usuario o contraseña incorrecta" });

                //  datos para token y tiempo 
                string Token = _token.GenerateToken(user.Id, 1);

                //  crea y guarda el refreshToken en BD
                var refreshToken = await _serviceToken.CreateToken(user.Id);


                var result = new UserResDto
                {
                    IdUser = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Message = "Bienvenido",
                };

                Response.Cookies.Append("AuthToken", Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMonths(2)
                });

                return Ok(result);

            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "/api/session/login", 500, "Server error");
            }
        }


        /// <summary>
        /// refresca token de acceso de usuarios 
        /// </summary>
        /// <returns></returns>
        /// 
        /// 
        [HttpGet]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var data = await _serviceToken.RefreshToken(refreshToken);
            if (data == null)
                return Unauthorized();

            var newAccessToken = _token.GenerateToken(data, 1);
            Response.Cookies.Append("AuthToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new { message = "Token renovado correctamente" });
        }


        /// <summary>
        /// Registra un nuevo usuario en el sistema 
        /// </summary>
        /// <param name="User"></param>
        /// <returns>Usuario creado</returns>
        /// <remarks>
        /// Sample resquest:
        /// 
        ///     POST 
        ///     {
        ///         "Name": "Gustavo Bernal",
        ///         "Email": "gustavober98@gmail.com",
        ///         "Password": "12345678",
        ///         "Role": "usuario"
        ///     }
        /// </remarks>
        /// <response code="201">Usuario Creado</response>
        /// <response code="400">Algo Fallo</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("sign_in")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> SingIn([FromForm] UserDto User)
        {
            try
            {
                // validacion que los campos no esten vacios
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Se valida que el correo no exista en la base de datos
                if (await _mailService.ValidaMail(User.Email)) return BadRequest(new ResponseDto { Message = "El correo ya se encuentra registrado" });

                var resultado = await _service.SignIn(User);
                // validacion que se cree el usuario 
                if (resultado == null) return BadRequest(new ResponseDto { Message = "No se pudo crear usuario" });

                var result = _notificationMail.Notificar(new MailModel
                {
                    Mail = User.Email,
                    Motivo = "Bienvenido a Notas",
                    Message = "<h1><strong>Bienvenido a Notas</strong></h1><br> <div>Esta es una red donde puedes agregar tus notas y plasmar lo que piensas, lo que sientes, las cosas que te rodean y los buenos momentos que quieras narrar..</div>",
                });

                // crea token se session
                var Token = _token.GenerateToken(resultado, 1);

                //  crea y guarda el refreshToken en BD
                var refreshToken = await _serviceToken.CreateToken(resultado);

                Response.Cookies.Append("AuthToken", Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMonths(2)
                });

                return CreatedAtAction(
                    nameof(Login),
                    new { Mail = "user@example.com", Password = "***********" },
                    new UserResDto
                    {
                        Email = User.Email,
                        Name = User.Name,
                        Message = "Usuario Creado",
                        IdUser = resultado,
                    }
                 );
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "/api/session/singin", 500, "Server error");
            }
        }

        /// <summary>
        /// Cambia contraseña del usuario con la validacion del id de usuario
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize]
        [Route("change_password/{idUser}")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ChangesPasswords(string idUser, [FromForm] PasswordDto pass)
        {
            try
            {
                if (string.IsNullOrEmpty(idUser)) return BadRequest(new ResponseDto { Message = "Ingrese un id de usuario" });
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (await _service.ChangePassword(idUser, pass)) return Ok(new ResponseDto
                {
                    Message = "Contraseña cambiada correctamente"
                });

                return BadRequest(new ResponseDto
                {
                    Message = "No se pudo cambiar la contraseña"
                });
            }
            catch (System.Exception ex)
            {
                return Problem($"Hubo un error inesperado: {ex.Message}", "/api/session/choose_password", 500, "Server error");
            }
        }


        [HttpGet]
        [Route("log_out")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();


            // elimina datos de session 
            var response = await _serviceToken.DeleteTokenRefresh(refreshToken);
            if (!response) return BadRequest(new { Message = "fallo al cerrar sesion" });

            // se limpian las cookies con la session 
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var exp = User.FindFirst("exp")?.Value;

            if (jti != null && exp != null)
            {
                var expiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;
                _blackList.Revoke(jti, expiration);
            }

            // 🧹 Borrar ambas cookies
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-2),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("AuthToken", "", cookieOptions);
            Response.Cookies.Append("RefreshToken", "", cookieOptions);

            _logger.LogInformation("usuario cerro session");
            return Ok(new { message = "Sesión cerrada correctamente" });
        }

        [HttpGet]
        [Authorize]
        [Route("close_sesion/{token}")]
        public async Task<IActionResult> DeleteSessions(string token)
        {
            if (token == null) return BadRequest(new { Message = "no existe token" });
            var response = await _serviceToken.DeleteTokenRefresh(token);

            if (!response) return BadRequest(new { Message = "fallo al cerrar sesion" });
            return Ok(new { Message = "Sesion cerrada" });
        }
    }
}