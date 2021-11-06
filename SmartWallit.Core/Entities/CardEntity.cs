using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Core.Entities
{
    public class CardEntity : BaseEntity
    {
        public string CardNumber { get; set; }
        public int ExpirationYear { get; set; }
        public int ExpirationMonth { get; set; }
        public int Cvv { get; set; }
        public string CardNickname { get; set; }
        public int WalletId { get; set; }
        public string CardHash { get; set; }
        public byte[] CardSalt { get; set; }

    }
}
