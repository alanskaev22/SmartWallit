using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Core.Entities
{
    public class CardEntity : BaseEntity
    {
        private string _CardNumber;
        public string CardNumber 
        {
            get { return _CardNumber[..4] + "." + _CardNumber[^4..]; }
            set { _CardNumber = value;  }
        }
        public int ExpirationYear { get; set; }
        public int ExpirationMonth { get; set; }
        public int Cvv { get; set; }
        public string CardNickname { get; set; }
        public int WalletId { get; set; }
    }
}
