using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<CardEntity> GetCardById(string userId, int cardId);
        Task<CardEntity> CreateCard(string userId, CardEntity card);
        Task<List<CardEntity>> GetCards(string userId);
        Task<bool> DeleteCard(string userId, int cardId);
    }
}
