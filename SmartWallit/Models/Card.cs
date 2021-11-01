using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class Card
    {
        private string _CardNumber;
        public string CardNumber
        {
            get { return _CardNumber[^4..]; }
            set { _CardNumber = value; }
        }

        public int ExpirationYear { get; set; }
        public int ExpirationMonth { get; set; }
        public string CardNickname { get; set; }
    }
}
