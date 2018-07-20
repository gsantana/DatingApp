using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            Repo = repo;
            Config = config;
        }

        private IAuthRepository Repo { get; }
        public IConfiguration Config { get; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegister userForResister)
        {
            if (!string.IsNullOrEmpty(userForResister.Username))
                userForResister.Username = userForResister.Username.ToLower();

            if (await Repo.UserExistsAsync(userForResister.Username))
                ModelState.AddModelError("Username", "User is already taken");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userForResister.Username = userForResister.Username.ToLower();

            var userToCreate = new User() { Username = userForResister.Username };
            var createUser = await Repo.Register(userToCreate, userForResister.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLogin userForLogin)
        {
            var userFromRepo = Repo.Login(userForLogin.Username.ToLower(), userForLogin.Password);

            //throw new Exception("error bla bla bla");
            if (userFromRepo == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
                }
                ),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenstring = tokenHandler.WriteToken(token);

            return Ok(new { tokenstring });
        }


    }
}
