﻿using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly WalletContext _walletContext;

        public CardRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        public async Task<CardEntity> CreateCard(string userId, CardEntity card)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure wallet exists for the user.", nameof(userId));
            }

            card.WalletId = wallet.Id;

            var allcards = await _walletContext.Cards.Where(x => x.WalletId == card.WalletId).ToListAsync();

            var cardExists = allcards.FirstOrDefault(c => $"{c.CardNumber[..4]}.{c.CardNumber[^4..]}" == card.CardNumber);

            if (cardExists != null) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Card already exists.");


            await _walletContext.Cards.AddAsync(card);

            await _walletContext.SaveChangesAsync();

            return card;
        }

        public async Task<bool> DeleteCard(string userId, int cardId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));
            }

            var card = await _walletContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.WalletId == wallet.Id);

            if(card == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Card with id {cardId} Not Found.", nameof(cardId));

            _walletContext.Cards.Remove(card);

            await _walletContext.SaveChangesAsync();

            return true;

        }

        public async Task<CardEntity> GetCardById(string userId, int cardId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));
            }

            var card = await _walletContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.WalletId == wallet.Id);

            return card ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Card with id {cardId} Not Found.", nameof(cardId));
        }

        public async Task<List<CardEntity>> GetCards(string userId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));
            }

            var cards = await _walletContext.Cards.Where(c => c.WalletId == wallet.Id).ToListAsync();

            return cards ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, $"No cards were found for user id {userId}.");
        }
    }
}
