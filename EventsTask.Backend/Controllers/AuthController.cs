using EventsTask.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventsTask.Application.Common.Dtos;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventsTask.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
        {
   
            _usersService = usersService;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _usersService.Login(username, password);
            Response.Cookies.Append("token", token.AccessToken);
            return Ok(token);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            await _usersService.Register(username, password);


            return Ok();
        }

        [HttpPost("/refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _usersService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}

