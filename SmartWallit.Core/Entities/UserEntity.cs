using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWallit.Core.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Column(TypeName="date")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Password { get; set; }

        public WalletEntity Wallet { get; set; }
        public int? WalletId { get; set; }
    }
}
