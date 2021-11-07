using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly WalletContext _walletContext;

        public LogRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        public async Task Log(LogEntity log)
        {
            await _walletContext.Logs.AddAsync(log);
            await _walletContext.SaveChangesAsync();
        }
    }
}
