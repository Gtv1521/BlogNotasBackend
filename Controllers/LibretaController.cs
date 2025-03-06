using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Dto.Books;
using BackEndNotes.Services;
using Microsoft.AspNetCore.Mvc;

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
                return StatusCode(500, ex.Message);
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