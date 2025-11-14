using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Interfaces;
using BlogNotasBackend.Dto.Notifications;
using BlogNotasBackend.Interfaces.Principals;
using BlogNotasBackend.Models;
using BlogNotasBackend.requests;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using src.Dto;
using src.Models;
using src.Services;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShareNoteController : ControllerBase
    {
        private readonly ShareNoteService _service;
        public ShareNoteController(ShareNoteService service)
        {
            _service = service;
        }


        //  obtiene una nota compartida
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest(new ResponseDto { Message = "Debe enviar el id" });
            var response = await _service.GetShare(id);
            if (response == null) return NotFound(new ResponseDto { Message = "No se encontraron datos" });
            return Ok(response);
        }

        [HttpGet]
        [Route("{idNote}/{page}")]
        public async Task<IActionResult> GetAll(string idNote, int page)
        {
            if (string.IsNullOrEmpty(idNote) || int.IsNegative(page)) return BadRequest(new ResponseDto { Message = "Debe enviar todos los datos" });

            var res = await _service.GetAllShare(idNote, page);
            if (res.Count() == 0) return NotFound(new ResponseDto { Message = "No hay notas compartidas" });
            return Ok(res);
        }

        [HttpGet]
        [Route("fiter/{filter}/{id}")]
        public async Task<IActionResult> Filter(string filter, string id)
        {
            if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(id)) return BadRequest(new ResponseDto { Message = "Debe de enviar todos los datos" });
            var response = await _service.Filter(filter, id);
            return Ok(response);
        }

        [HttpPost]
        // [Route("")]
        public async Task<IActionResult> New([FromBody] ShareNoteDto data)
        {
            // if (!ModelState.IsValid) return BadRequest(ModelState);
            // llama al service para crear la nota compartida
            var model = new ShareNoteModel
            {
                NoteId = data.IdNote,
                IdUser = data.IdUser,
                IdUserReference = data.IdReferido,
                ReadPermits = data.ReadPermits,
                WritePermits = data.WritePermits
            };


            var insert = await _service.NewShare(model);
            if (insert == null)
                return BadRequest(new ResponseDto { Message = "No se pudo crear la nota compartida" });

            var information = await _service.GetShare(insert);
            return Created(nameof(Get), new { id = insert });
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, [FromQuery] ShareNoteModel data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _service.UpdatePermits(id, data);
            if (!response) return NotFound(new ResponseDto { Message = "El usuario no se encontro" });
            return Ok(new ResponseDto { Message = "data actualizada" });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest(new ResponseDto { Message = "Debe enviar todos los datos" });
            var response = await _service.DeleteShare(id);
            if (!response) return NotFound(new ResponseDto { Message = "No se encuentra la nota" });
            return Ok(new ResponseDto { Message = "Enlace terminado" });
        }
    }
}