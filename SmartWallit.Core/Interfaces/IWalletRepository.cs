﻿using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Core.Interfaces
{
    public interface IWalletRepository
    {
        Task<WalletEntity> CreateWallet(string userId);
        Task<WalletEntity> GetWallet(string userId);
        Task<bool> DeleteWallet(string userId);
    }
}
