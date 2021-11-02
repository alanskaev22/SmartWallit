using System;
using System.Threading.Tasks;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;

namespace SmartWallit.Infrastructure.Data.Repositories
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
            var result = await _walletContext.Users.FindAsync(id);

            return result ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, $"User with id: {id} Not Found.", nameof(id));
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            await _walletContext.Users.AddAsync(user);

            await _walletContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _walletContext.Users.FindAsync(id);
                _walletContext.Remove(user);

                await _walletContext.SaveChangesAsync();
            }
            catch
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, $"User with id {id} Not Found. User could already have been deleted.", nameof(id));
            }

        }
    }
}
