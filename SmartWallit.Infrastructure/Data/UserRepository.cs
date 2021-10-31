using System;
using System.Threading.Tasks;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Interfaces;

namespace SmartWallit.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly WalletContext _walletContext;

        public UserRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            return await _walletContext.Users.FindAsync(id);
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            await _walletContext.Users.AddAsync(user);

            await _walletContext.SaveChangesAsync();

            return user;
        }
    }
}
