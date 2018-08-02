using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private object _repo;
        private object _mapper;

        private IDatingRepository Repo { get; }
        public IMapper Mapper { get; }

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            Repo = repo;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await Repo.GetUsers();
            var usersToReturn = Mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await Repo.GetUser(id);

            var userToReturn = Mapper.Map<UserForDetailsDto>(user);

            return Ok(userToReturn);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await Repo.GetUser(id);

            Mapper.Map(userForUpdateDto, userFromRepo);

            if (Repo.SaveAll().Result)
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}
