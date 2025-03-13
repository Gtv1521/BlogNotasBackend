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
        public LibretaController(BookService service)
        {
            _service = service;
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
                return Ok(await _service.ViewAllBooks(iduser, pagina));
            }
            catch (System.Exception ex)
            {
                return Problem("Algo fallo", $"/view_books/{iduser}/{pagina}", 500, ex.Message, "Server error");
            }
        }

        /// <summary>
        /// Crea una libreta
        /// </summary>
        /// <param name="libro"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create_book")]
        public async Task<IActionResult> CreatedBook(BooksDto libro)
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
        public async Task<IActionResult> UpdateName(string idLibreta, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(idLibreta) && string.IsNullOrEmpty(name)) return BadRequest(new ResponseNoteDto { Message = "Debe enviar todos los datos " });
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