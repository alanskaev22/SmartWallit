using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _walletContext;

        public WalletRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        public async Task<WalletEntity> CreateWallet(WalletEntity wallet)
        {
            await _walletContext.AddAsync(wallet);
            await _walletContext.SaveChangesAsync();

            return wallet;
        }
    }
}
