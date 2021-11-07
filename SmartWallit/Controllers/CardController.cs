using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using SmartWallit.Extensions;
using SmartWallit.Models;
using System.Threading.Tasks;

namespace SmartWallit.Controllers
{
    [Authorize]
    public class CardController : BaseApiController
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public CardController(ICardRepository cardRepository, IMapper mapper, ITokenService tokenService)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCard([FromQuery] int cardId)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardEntity = await _cardRepository.GetCardById(userId, cardId);

            return Ok(_mapper.Map<CardResponse>(cardEntity));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCard(CardRequest card)
        {
            card.ValidateCardExpiration();

            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardEntity = await _cardRepository.CreateCard(userId, _mapper.Map<CardEntity>(card));

            return Ok(_mapper.Map<CardResponse>(cardEntity));
        }

        [HttpPut]
        [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCard(UpdateCardRequest card)
        {
            card.ValidateCardExpiration();

            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardEntity = await _cardRepository.UpdateCard(userId, _mapper.Map<CardEntity>(card));

            return Ok(_mapper.Map<CardResponse>(cardEntity));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardDeleted = await _cardRepository.DeleteCard(userId, cardId);

            return cardDeleted ? Ok() : BadRequest();
        }
    }
}
