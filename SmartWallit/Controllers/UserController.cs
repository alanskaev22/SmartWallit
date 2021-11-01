using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using SmartWallit.Infrastructure.Data;
using SmartWallit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json"), Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return Ok(_mapper.Map<UserEntity, User>(user));

        }

        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> AddUser(NewUserRequest user)
        {
            return Ok(await _userRepository.AddUserAsync(_mapper.Map<NewUserRequest, UserEntity>(user)));
        }

        [HttpDelete("{id}")]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);

            return Ok();
        }

    }
}
