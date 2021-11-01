using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Core.Entities
{
    public class CardEntity : BaseEntity
    {
        [Required, MaxLength(19), MinLength(8)]
        public string CardNumber { get; set; }
        [Required, StringLength(4)]
        public int ExpirationYear { get; set; }
        [Required, StringLength(2)]
        public int ExpirationMonth { get; set; }
        [Required, MaxLength(4),MinLength(3)]
        public int Cvv { get; set; }
        public string CardNickname { get; set; }
        [Required]
        public int WalletId { get; set; }
    }
}
