using System;
using System.Collections.Generic;

namespace SmartWallit.Core.Entities
{
    public class WalletEntity : BaseEntity
    {
        public decimal Balance { get; set; }
        public int? CardId { get; set; }

        public ICollection<CardEntity> Cards { get; set; }

    }
}
