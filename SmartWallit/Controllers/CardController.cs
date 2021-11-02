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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardController(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> GetCard(int userId, [FromQuery] int cardId)
        {
            var cardEntity = await _cardRepository.GetCardById(userId, cardId);

            return Ok(_mapper.Map<CardEntity, Card>(cardEntity));
        }

        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorDetails))]
        public async Task<IActionResult> AddCard(int userId, CardRequest card)
        {
            var cardEntity = await _cardRepository.CreateCard(userId, _mapper.Map<CardRequest, CardEntity>(card));

            return Ok(_mapper.Map<CardEntity, Card>(cardEntity));
        }
    }
}
