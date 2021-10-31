using System;
using System.Collections.Generic;

namespace SmartWallit.Core.Entities
{
    public class WalletEntity : BaseEntity
    {
        public decimal Balance { get; set; }
        public List<CardEntity> Cards { get; set; }
        public UserEntity User { get; set; }
    }
}
