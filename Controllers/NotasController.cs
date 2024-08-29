using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Amazon.Runtime.Internal.Auth;
using Microsoft.AspNetCore.Mvc;
using Notas_Back.Dto;
using Notas_Back.Models;
using Notas_Back.Services;

namespace Notas_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasController : ControllerBase
    {
        private readonly NotasService _service;
        private readonly UsuariosService _user;

        public NotasController(NotasService notasService, UsuariosService usuariosService)
        {
            _service = notasService;
            _user = usuariosService;
        }

        /// <summary>
        /// Muestra todas las notas de un usuario 
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("allNotas/{IdUser}")]
        public async Task<IActionResult> allNotes(string IdUser)
        {
            List<Notas> Result = await _service.allNotasByUser(IdUser);
            if (Result.Count != 0)
            {
                return Ok(Result);
            }
            else
            {
                return Ok(new NoData
                {
                    status = 200,
                    mensaje = "NO hay Notas"
                });
            }
        }

        /// <summary>
        /// Muestra los datos de una nota 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Nota/{id}")]
        public async Task<IActionResult> getNota(string id)
        {
            if (id == null)
            {
                return BadRequest(new NoData
                {
                    status = 404,
                    mensaje = "Envia un identificador de nota"
                });
            }
            else
            {
                return Ok(await _service.Get(id));
            }
        }

        /// <summary>
        /// Inserta una nueva nota
        /// </summary>
        /// <param name="notas"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("newNote")]
        public async Task<IActionResult> newNotes(Notas notas)
        {
            if (notas.IdUser != null)
            {
                Usuarios user = await _user.Get(notas.IdUser);
                if (user != null)
                {
                    await _service.Post(notas);
                    return Created("Created", true);
                }
                else
                {
                    return Ok(new NoData
                    {
                        status = 200,
                        mensaje = "Usuario no existe"
                    });
                }
            }
            else
            {
                return Ok(new NoData
                {
                    status = 200,
                    mensaje = "Debe enviar datos"
                });
            }
        }

        /// <summary>
        /// Actualiza los datos que contiene la nota 
        /// </summary>
        /// <param name="notas"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ActualizaNota/{id}")]
        public async Task<IActionResult> updateNota(NotasUpdateDto notas, string id)
        {
            if (notas == null || id == null)
            {
                return BadRequest(new NoData
                {
                    status = 404,
                    mensaje = "Debe enviar todos los datos"
                });
            }
            else
            {
                await _service.Update(new Notas
                {
                    IdNota = id, 
                    Titulo = notas.Titulo,
                    Contenido = notas.Contenido,
                    FechaUpdate = notas.FechaUpdate,

                });
                return Ok(new NoData
                {
                    status = 200,
                    mensaje = "Datos actualizados con éxito"
                });
            }
        }

        /// <summary>
        /// Borra una nota con el id de la nota
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteNota/{id}")]
        public async Task<IActionResult> deleteNote(string id)
        {
            if (id != null)
            {
                await _service.Delete(id);
                return Ok(new NoData
                {
                    status = 200,
                    mensaje = "Nota Eleminada con exito"
                });
            }
            else
            {
                return Ok(new NoData
                {
                    status = 200,
                    mensaje = "Inserte el id que se quiere borrar"
                });
            }

        }

    }
}