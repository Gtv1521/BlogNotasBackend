using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;
using BackEndNotes.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEndNotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // private readonly ILogger _log;
        private readonly UserService _service;

        public UserController(UserService service)
        {
            // _log = log;
            _service = service;
        }


        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="user"></param>
        // /// <returns></returns>
        // [HttpPost]
        // [Route("NewUser")]
        // [Consumes("multipart/form-data")]

        // public async Task<IActionResult> SignIn([FromBody] UserDto user)
        // {
        //     try
        //     {
        //         return Ok(await _service.CreateUser(user));
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return Problem(ex.Message, "/api/plants/view_all", 500, "Server error");
        //     }
        // }
    }
}