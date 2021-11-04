using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Entities.Identity;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _walletContext;
        private readonly UserManager<AppUser> _userManager;

        public WalletRepository(WalletContext walletContext, UserManager<AppUser> userManager)
        {
            _walletContext = walletContext;
            _userManager = userManager;
        }

        public async Task<WalletEntity> CreateWallet(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, $"No user was found with id {userId}.");
            }

            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet != null)
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Wallet already exists for this user.");
            }
            
            wallet = new WalletEntity {UserId = userId };

            await _walletContext.Wallets.AddAsync(wallet);
            await _walletContext.SaveChangesAsync();

            return wallet;
        }

        public async Task<WalletEntity> GetWallet(string userId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            return wallet ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, "No wallet was found for this user.");
        }

        public async Task<bool> DeleteWallet(string userId)
        {
            var success = false;

            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var cards = await _walletContext.Cards.Where(c => c.WalletId == wallet.Id).ToListAsync();
            
            using var transaction = _walletContext.Database.BeginTransaction();
            try
            {
                _walletContext.Cards.RemoveRange(cards);

                _walletContext.Wallets.Remove(wallet);

                await _walletContext.SaveChangesAsync();

                await transaction.CommitAsync();

                success = true;
            }
            catch
            {
            }

            return success;
        }
    }
}
