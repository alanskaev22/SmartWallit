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
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _walletContext;

        public WalletRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        public async Task<WalletEntity> CreateWallet(int userId)
        {
            var user = await _walletContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, $"No user was found with id {userId}.");
            }

            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet != null)
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Wallet already exists for this user.");
            }

            wallet.UserId = userId;

            await _walletContext.Wallets.AddAsync(wallet);
            await _walletContext.SaveChangesAsync();

            return wallet;
        }

        public async Task<WalletEntity> GetWallet(int userId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            return wallet ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, "No wallet was found for this user.");
        }
    }
}
