using Microsoft.EntityFrameworkCore;
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

        public async Task<CardEntity> CreateCard(int userId, CardEntity card)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure wallet exists for the user.", nameof(userId));
            }

            card.WalletId = wallet.Id;

            await _walletContext.Cards.AddAsync(card);

            await _walletContext.SaveChangesAsync();

            return card;
        }

        public async Task<CardEntity> GetCardById(int userId, int cardId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if(wallet == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));
            }

            var card = await _walletContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.WalletId == wallet.Id);

            return card ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Card with id {cardId} Not Found.", nameof(cardId));
        }

        public async Task<List<CardEntity>> GetCards(int userId)
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
