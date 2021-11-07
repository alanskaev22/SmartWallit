using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using SmartWallit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWallit.Controllers
{
    [Authorize]
    public class WalletController : BaseApiController
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public WalletController(IWalletRepository walletRepository, IMapper mapper, ICardRepository cardRepository, ITokenService tokenService)
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
            _cardRepository = cardRepository;
            _tokenService = tokenService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWallet()
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var walletEntity = await _walletRepository.GetWallet(userId);
            var cardsEntity = await _cardRepository.GetCards(userId);

            var wallet = _mapper.Map<Wallet>(walletEntity);
            var cards = _mapper.Map<List<CardResponse>>(cardsEntity);

            wallet.Cards = cards;

            return Ok(wallet);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWallet()
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var walletEntity = await _walletRepository.CreateWallet(userId);
            var cardsEntity = await _cardRepository.GetCards(userId);

            var wallet = _mapper.Map<Wallet>(walletEntity);
            var cards = _mapper.Map<List<CardResponse>>(cardsEntity);

            wallet.Cards = cards;

            return Ok(wallet);
        }

        [HttpPost("balance/add")]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFunds(FundsTransfer request)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");
            var email = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userEmail");

            var walletEntity = await _walletRepository.AddFunds(userId, request.CardId, request.Amount, email);
            var cardsEntity = await _cardRepository.GetCards(userId);

            var wallet = _mapper.Map<Wallet>(walletEntity);
            var cards = _mapper.Map<List<CardResponse>>(cardsEntity);

            wallet.Cards = cards;

            return Ok(wallet);
        }

        [HttpPost("balance/withdraw")]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        public async Task<IActionResult> WithdrawFunds(FundsTransfer request)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");
            var email = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userEmail");

            var walletEntity = await _walletRepository.WithdrawFunds(userId, request.CardId, request.Amount, email);
            var cardsEntity = await _cardRepository.GetCards(userId);

            var wallet = _mapper.Map<Wallet>(walletEntity);
            var cards = _mapper.Map<List<CardResponse>>(cardsEntity);

            wallet.Cards = cards;

            return Ok(wallet);
        }

    }
}
