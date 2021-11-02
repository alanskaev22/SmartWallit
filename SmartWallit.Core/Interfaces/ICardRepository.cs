using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<CardEntity> GetCardById(int userId, int cardId);
        Task<CardEntity> CreateCard(int userId, CardEntity card);
        Task<List<CardEntity>> GetCards(int userId);
    }
}
