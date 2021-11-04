using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Entities.Identity;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using SmartWallit.Extensions;
using SmartWallit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartWallit.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json"), Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            
            var response = _mapper.Map<AppUser, AuthenticateResponse>(user);
           
            response.Token = _tokenService.CreateToken(user);

            return Ok(response);
        }

        [HttpGet("address")]
        [Authorize]
        [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);

            var response = _mapper.Map<Address, AddressModel>(user?.Address ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, "Address not found"));

            return Ok(response);
        }

        [HttpPut("address")]
        [Authorize]
        [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> UpdateUserAddress(AddressModel address)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);

            user.Address = _mapper.Map<AddressModel, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressModel>(user.Address));

            throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Error Updating Address.");
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "User not authorized");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if(!result.Succeeded) throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "User not authorized");
            var response = _mapper.Map<AppUser, AuthenticateResponse>(user);
            
            response.Token = _tokenService.CreateToken(user);

            return Ok(response);

        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = _mapper.Map<RegisterRequest, AppUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new CustomException(System.Net.HttpStatusCode.BadRequest);
            
            var response = _mapper.Map<AppUser, AuthenticateResponse>(user);

            response.Token = _tokenService.CreateToken(user);

            return Ok(response);

        }
    }
}
