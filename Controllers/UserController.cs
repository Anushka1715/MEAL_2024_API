﻿using MEAL_2024_API.Context;
using MEAL_2024_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEAL_2024_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public UserController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;

        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if(userObj == null)
            {
                return BadRequest();
            }
            var user = await _authContext.Users.
                FirstOrDefaultAsync(x => x.EmailId == userObj.EmailId && x.Password == userObj.Password);
            if(user == null)
            {
                return NotFound(new { Message = "User Not Found!" });
            }

            return Ok(new
            {
                Message="Login Success!"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if(userObj == null)
            {
                return BadRequest();
            }

            userObj.UserId = new Guid();
            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "User Registered!"
            });
        }


    }
}
