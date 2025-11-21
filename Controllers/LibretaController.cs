using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Dto.Books;
using BackEndNotes.Services;
using Microsoft.AspNetCore.Mvc;
using BackEndNotes.Dto;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;

namespace BackEndNotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibretaController : ControllerBase
    {
        private readonly BookService _service;
        private readonly NotesService _notes;
        public LibretaController(BookService service, NotesService notes)
        {
            _service = service;
            _notes = notes;
        }

        /// <summary>
        ///  muestra todas las libretas que estas ligadas al usuario 
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("view_books/{iduser}/{pagina}")]
        public async Task<IActionResult> GetLibretasUser(string iduser, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(iduser)) return BadRequest(new ResponseDto { Message = "Los campos son requeridos" });
                var response = await _service.ViewAllBooks(iduser, pagina);

                var data = await Task.WhenAll(response.Select(async item => new
                {
                    idLibreta = item.IdLibreta,
                    nombre = item.Nombre,
                    idUser = item.IdUser,
                    NotesCount = await _notes.CountNotes(item.IdLibreta.ToString())
                }));

                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return Problem("Algo fallo", $"/view_books/{iduser}/{pagina}", 500, ex.Message, "Server error");
            }
        }


        [HttpGet]
        [Route("view_book/{idLibreta}")]
        public async Task<IActionResult> GetLibreta(string idLibreta)
        {
            var responde = await _service.ViewOneBook(idLibreta);
            if (responde == null) return NotFound("Not found data");
            return Ok(responde);
        }

        [HttpGet]
        [Route("books_count/{idUser}")]
        public async Task<IActionResult> Countlibreta(string idUser)
        {
            try
            {
                var response = await _service.Countlibreta(idUser);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem("Algo fallo", $"/create-book", 500, ex.Message, "Server error");
            }
        }

        [HttpGet]
        [Route("filter/{id}")]
        public async Task<IActionResult> Filter(string id, [FromQuery] string filter)
        {
            if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(id)) return BadRequest(new ResponseDto { Message = "Se requieren todos los datos" });
            return Ok(await _service.Filter(filter, id));
        }


        /// <summary>
        /// Crea una libreta
        /// </summary>
        /// <param name="libro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create_book")]
        public async Task<IActionResult> CreatedBook([FromBody] BooksDto libro)
        {
            try
            {
                var id = await _service.Create(libro);
                return CreatedAtAction(null, new ResponseNoteDto
                {
                    Message = "libreta creada",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return Problem("Algo fallo", $"/create-book", 500, ex.Message, "Server error");
            }
        }

        /// <summary>
        /// Actualiza el nombre de una libreta
        /// </summary>
        /// <param name="idLibreta"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("update_name/{idLibreta}")]
        public async Task<IActionResult> UpdateName(string idLibreta, [FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrEmpty(idLibreta) || string.IsNullOrEmpty(name)) return BadRequest(new ResponseNoteDto { Message = "Debe enviar todos los datos " });
                return Ok(await _service.UpdateBook(idLibreta, name));
            }
            catch (System.Exception ex)
            {
                return Problem("Algo fallo", $"/update_name/{idLibreta}", 500, ex.Message, "Server error");
            }
        }

        /// <summary>
        /// Delete one book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("remove_book/{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return BadRequest("Debe enviar el id del libro");
                var result = await _service.Remove(id);
                if (!result) return NotFound("Libro no encontrado");
                return Ok("Libro eliminado correctamente");
            }
            catch (System.Exception ex)
            {
                return Problem("algo fallo", $"/remove_book/{id}", 500, ex.Message, "Server error");
            }
        }

    }
}