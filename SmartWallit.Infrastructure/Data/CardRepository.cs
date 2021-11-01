using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data
{
    public class CardRepository : ICardRepository
    {
        private readonly WalletContext _walletContext;

        public CardRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }
        public async Task<CardEntity> CreateCard(CardEntity card)
        {
            await _walletContext.Cards.AddAsync(card);

            await _walletContext.SaveChangesAsync();

            return card;
        }
    }
}
