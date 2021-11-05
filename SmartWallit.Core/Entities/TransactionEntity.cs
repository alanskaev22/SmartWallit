using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Core.Entities
{
    public class TransactionEntity : BaseEntity
    {
        public int WalletId { get; set; }
        public string Email { get; set; }
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
