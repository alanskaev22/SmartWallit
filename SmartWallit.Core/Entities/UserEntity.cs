using System;
using System.Collections.Generic;

namespace SmartWallit.Core.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }

        public WalletEntity Wallet { get; set; }
        public int WalletId { get; set; }
    }
}
