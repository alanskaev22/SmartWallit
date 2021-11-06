using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Entities.Identity;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using SmartWallit.Extensions;
using SmartWallit.Models;
using System.Threading.Tasks;

namespace SmartWallit.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IWalletRepository _walletRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ITokenService tokenService, IWalletRepository walletRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _walletRepository = walletRepository;
        }

        [Authorize]
        [HttpGet("token/refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await _userManager.FindByEmailFromClaims(HttpContext.User);

            return Ok(new { token = _tokenService.CreateToken(user) });
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountDetails()
        {
            var user = await _userManager.FindByEmailWithAddressFromClaims(HttpContext.User);

            var response = _mapper.Map<AppUser, Account>(user ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, "Account not found"));

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAccount(Account account)
        {
            var user = await _userManager.FindByEmailWithAddressFromClaims(HttpContext.User);

            user.FirstName = account.FirstName;
            user.LastName = account.LastName;
            user.DateOfBirth = account._DateOfBirth.Value;
            user.Address = _mapper.Map<AddressModel, Address>(account.Address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AppUser, Account>(user));

            throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Error Updating Account.");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.FindByEmailFromClaims(HttpContext.User);

            if (user == null) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Bad Request");

            var wallet = await _walletRepository.GetWallet(user.Id);

            if (wallet.Balance > 0) throw new CustomException(System.Net.HttpStatusCode.BadRequest, $"Wallet has a balance of {wallet.Balance}. Transfer remaining balance before deleting account.");

            var result = await _walletRepository.DeleteWallet(user.Id);

            if (!result) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Bad Request");

            var userResult = await _userManager.DeleteAsync(user);

            if (!userResult.Succeeded) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Bad Request");

            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "User not authorized");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded) throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "User not authorized");

            return Ok(new { token = _tokenService.CreateToken(user) });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Account already exists.");

            user = _mapper.Map<RegisterRequest, AppUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new CustomException(System.Net.HttpStatusCode.BadRequest);

            return Ok(new { token = _tokenService.CreateToken(user) });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!result.Succeeded) throw new CustomException(System.Net.HttpStatusCode.BadRequest);

            return Ok(new { token = _tokenService.CreateToken(user) });
        }
    }
}
