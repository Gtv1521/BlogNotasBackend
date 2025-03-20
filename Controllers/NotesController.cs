using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Models.Notes;
using BackEndNotes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BackEndNotes.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NotesService _service;
        public NotesController(NotesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Trae datos de una nota registrada 
        /// </summary>
        /// <param name="idNote"></param>
        /// <returns>Nota</returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     GET 
        ///     {
        ///         "idNote": "679cf14b5b377829f27c5df1"
        ///     }
        /// </remarks>
        /// <response code="200">Nota.</response>
        /// <response code="404">No encontrada</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("one_note/{idNote}")]
        [Consumes("application/json", "multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ObtenerNotas(string idNote)
        {
            try
            {
                var notes = await _service.ObtenerNota(idNote);
                if (notes == null) return NotFound(new ResponseDto { Message = "No se encontro la nota" });
                return Ok(notes);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, $"api/Notes/one_note/{idNote}", 500, "Server error");
            }
        }

        /// <summary>
        /// Trae las notas del usuario
        /// </summary>
        /// <param name="IdLibreta"></param>
        /// <param name="pagina"></param>
        /// <returns>Notas Creadas por el usuario</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET
        ///     {
        ///         "idUser": "679c21ebc05a56196a429741"
        ///         "pagina": 1
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Lista notas</response>
        /// <response code="404">No hay data</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("all_notes/{IdLibreta}/{pagina}")]
        [Consumes("application/json", "multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NotesModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ObtenerNotasLibreta(string IdLibreta, int pagina)
        {
            try
            {
                var notes = await _service.ObtenerNotasUsuario(IdLibreta, pagina);
                if (notes.Count() == 0) return NotFound(new ResponseDto { Message = "No hay notas" });
                return Ok(notes);
            }
            catch (System.Exception)
            {
                return Problem("Hubo un error inesperado", $"api/notes/all_notes/{IdLibreta}", 500, "Server error");
            }
        }

        /// <summary>
        /// Crea una nota nueva en la base de datos 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Usuario creado</returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     GET 
        ///     {
        ///         "Title": "My First Note",
        ///         "Contenido": "Esta es la primera nota que agrego a mi blog, es parte de  mis pensamiento de los ultimos dias y hace parte de mi",
        ///         "IdUser": "679419a3f650f611c6ce3237",
        ///         "FechaCreacion": "2025-01-31T17:20:06.152Z",
        ///     }
        /// </remarks>
        /// <response code="201">Created successfully</response>
        /// <response code="400">No se pudo crear la nota</response>
        /// <response code="500">Error del servidor </response>
        [HttpPost]
        [Route("new_nota")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseNoteDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CrearNota([FromForm] NotesDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var idNota = await _service.CrearNota(model);
                if (string.IsNullOrEmpty(idNota)) return StatusCode(500);
                return CreatedAtAction(null, new ResponseNoteDto
                {
                    Message = "Nota creada",
                    Id = idNota
                });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, "api/Notes/new_nota", 500, "Server error");
            }

        }

        /// <summary>
        /// Actualiza el contenido y el titulo de la nota 
        /// </summary>
        /// <param name="idNote"></param>
        /// <param name="model"></param>
        /// <returns>Mensaje successfully</returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     GET /update_note/679cf72528458369a5f99176
        ///     {
        ///         "Title": "My first update",
        ///         "Contenido": "Hola!!, esta es la primera actualizacion que hago a un de mis notas creadas",
        ///     }
        /// </remarks>
        /// <response code="200">Actualizado con exito</response>
        /// <response code="400">No se pudo actualizar la nota</response>
        /// <response code="500">Server error</response>
        [HttpPatch]
        [Route("update_note/{idNote}")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateNotes(string idNote, [FromForm] UpdateNoteDto model)
        {
            try
            {
                var response = await _service.ActualizarNota(idNote, model);
                
                if (!response) return BadRequest(new ResponseDto {
                    Message = "No se pudo actualizar la nota"
                });

                return Ok(new ResponseDto {
                    Message = "Nota actualizada correctamente"
                });
            }   
            catch (System.Exception ex)
            {
                return Problem(ex.Message, $"api/Notes/update_note/{idNote}",500, "Server error");
            }
        }
        /// <summary>
        /// Eliminar una nota 
        /// </summary>
        /// <param name="idNote"></param>
        /// <returns>Mensaje verdadero o falso</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET
        ///     {
        ///         "idNote": "679cf14b5b377829f27c5df1"
        ///     }
        /// </remarks>
        /// <response code="200">Successfully</response>
        /// <response code="400">Error de entrada de datos</response>
        /// <response code="404">No encontrado</response>
        /// <response code="500">Server error</response>
        [HttpDelete]
        [Route("remove_note/{idNote}")]
        [Consumes("multipart/form-data", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> EliminarNota(string idNote)
        {
            try
            {
                if (string.IsNullOrEmpty(idNote)) return BadRequest(new { Message = "Falta el id de la nota" });
                var result = await _service.RemoveNote(idNote);
                if (!result) return NotFound(new ResponseDto { Message = "El id de la nota no existe" });
                return Ok(new ResponseDto { Message = "Nota eliminada correctamente" });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message, $"api/notes/remove_note/{idNote}", 500, "Server error");
            }
        }
    }
}