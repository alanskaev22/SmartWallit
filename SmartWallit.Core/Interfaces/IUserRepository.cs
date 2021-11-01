using System;
using System.Threading.Tasks;
using SmartWallit.Core.Entities;

namespace SmartWallit.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByIdAsync(int id);
        Task<UserEntity> AddUserAsync(UserEntity user);
        Task DeleteUserAsync(int id);
    }
}
