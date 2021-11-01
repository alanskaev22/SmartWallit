using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Core.Interfaces
{
    public interface IWalletRepository
    {
        Task<WalletEntity> CreateWallet(WalletEntity wallet);
    }
}
