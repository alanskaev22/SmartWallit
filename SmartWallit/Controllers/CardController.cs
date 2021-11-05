using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/[controller]")]
    [Consumes("application/json"), Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ErrorDetails))]
    [ApiController]
    public class CardController : ControllerBase
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
        [ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> GetCard([FromQuery] int cardId)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardEntity = await _cardRepository.GetCardById(userId, cardId);

            return Ok(_mapper.Map<CardEntity, Card>(cardEntity));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> AddCard(CardRequest card)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardEntity = await _cardRepository.CreateCard(userId, _mapper.Map<CardRequest, CardEntity>(card));

            return Ok(_mapper.Map<CardEntity, Card>(cardEntity));
        }

        [HttpDelete]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            var userId = _tokenService.GetClaimValueFromClaimsPrincipal(HttpContext.User, "userId");

            var cardDeleted = await _cardRepository.DeleteCard(userId, cardId);

            return cardDeleted ? Ok() : BadRequest();
        }
    }
}
