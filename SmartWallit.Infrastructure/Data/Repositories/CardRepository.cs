using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Enums;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly WalletContext _walletContext;
        private readonly IEncryptionService _encryptionService;

        public CardRepository(WalletContext walletContext, IEncryptionService encryptionService)
        {
            _walletContext = walletContext;
            _encryptionService = encryptionService;
        }

        public async Task<CardEntity> CreateCard(string userId, CardEntity card)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure wallet exists before adding cards.", nameof(userId));

            card.WalletId = wallet.Id;

            var allcards = await _walletContext.Cards.Where(x => x.WalletId == card.WalletId).ToListAsync();

            var cardExists = allcards.FirstOrDefault(c => _encryptionService.Decrypt(card.CardNumber, c.CardSalt, c.CardHash));

            if (cardExists != null) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Card already exists.");

            var encryptedCardNumber = _encryptionService.Encrypt(card.CardNumber);

            card.CardHash = encryptedCardNumber.Hash;
            card.CardSalt = encryptedCardNumber.Salt;

            card.CardBrand = GetCardBrand(card.CardNumber).ToString();

            // Leave only first and last 4 characters of a CardNumber;
            card.CardNumber = card.CardNumber.Replace(card.CardNumber[4..^4], ".");

            await _walletContext.Cards.AddAsync(card);

            await _walletContext.SaveChangesAsync();

            return card;
        }

        public async Task<bool> DeleteCard(string userId, int cardId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));

            var card = await _walletContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.WalletId == wallet.Id);

            if (card == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Card with id {cardId} Not Found.", nameof(cardId));

            _walletContext.Cards.Remove(card);

            await _walletContext.SaveChangesAsync();

            return true;

        }

        public async Task<CardEntity> GetCardById(string userId, int cardId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));

            var card = await _walletContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.WalletId == wallet.Id);

            return card ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Card with id {cardId} Not Found.", nameof(cardId));
        }

        public async Task<List<CardEntity>> GetCards(string userId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet for user with id {userId} Not Found. Make sure user has wallet.", nameof(userId));

            var cards = await _walletContext.Cards.Where(c => c.WalletId == wallet.Id).ToListAsync();

            return cards ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, $"No cards were found for user id {userId}.");
        }

        public async Task<CardEntity> UpdateCard(string userId, CardEntity cardToUpdate)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, $"Wallet does not exist, card not found.");

            var cardInDb = await _walletContext.Cards.FindAsync(cardToUpdate.Id);

            if (cardInDb == null || cardInDb.WalletId != wallet.Id) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Card Not Found.");

            cardInDb.CardNickname = cardToUpdate.CardNickname;
            cardInDb.ExpirationYear = cardToUpdate.ExpirationYear;
            cardInDb.ExpirationMonth = cardToUpdate.ExpirationMonth;
            cardInDb.Cvv = cardToUpdate.Cvv;

            _walletContext.Cards.Update(cardInDb);

            await _walletContext.SaveChangesAsync();

            return cardInDb;
        }

        private CardBrand GetCardBrand(string cardNumber)
        {
            if (Regex.IsMatch(cardNumber, VisaRegex)) return CardBrand.Visa;
            if (Regex.IsMatch(cardNumber, MasterCardRegex)) return CardBrand.MasterCard;
            if (Regex.IsMatch(cardNumber, AmexRegex)) return CardBrand.Amex;
            if (Regex.IsMatch(cardNumber, DinersClubRegex)) return CardBrand.DinersClub;
            if (Regex.IsMatch(cardNumber, DiscoverRegex)) return CardBrand.Discover;
            if (Regex.IsMatch(cardNumber, JCBRegex)) return CardBrand.JCB;

            return CardBrand.Unknown;

        }

        private string VisaRegex = @"^4[0-9]{6,}$";
        private string MasterCardRegex = @"^5[1-5] [0-9]{5,}| 222[1 - 9][0 - 9]{ 3,}| 22[3 - 9][0 - 9]{ 4,}| 2[3 - 6][0 - 9]{ 5,}| 27[01][0 - 9]{ 4,}| 2720[0 - 9]{ 3,}$";
        private string AmexRegex = @"^3[47][0 - 9]{ 5,}$";
        private string DinersClubRegex = @"^3(?:0[0 - 5] |[68][0 - 9])[0 - 9]{ 4,}$";
        private string DiscoverRegex = @"^6(?:011 | 5[0 - 9]{ 2})[0-9]{ 3,}$";
        private string JCBRegex = @"^(?: 2131 | 1800 | 35[0 - 9]{ 3})[0-9]{ 3,}$";
    }

}
