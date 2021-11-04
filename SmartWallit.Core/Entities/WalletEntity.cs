using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Core.Entities
{
    public class WalletEntity : BaseEntity
    {
        public decimal Balance { get; set; }
        public string UserId { get; set; }

    }
}
