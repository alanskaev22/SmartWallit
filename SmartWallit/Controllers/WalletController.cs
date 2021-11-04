using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
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
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public WalletController(IWalletRepository walletRepository, IMapper mapper, ICardRepository cardRepository)
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
            _cardRepository = cardRepository;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> GetWallet(string userId)
        {
            var walletEntity = await _walletRepository.GetWallet(userId);
            var cardsEntity = await _cardRepository.GetCards(userId);
            
            var wallet = _mapper.Map<WalletEntity, Wallet>(walletEntity);
            var cards = _mapper.Map<List<CardEntity>, List<Card>>(cardsEntity);

            wallet.Cards = cards;

            return Ok(wallet);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateWallet(string userId)
        {
            return Ok(await _walletRepository.CreateWallet(userId));
        }
    }
}
