using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class Wallet
    {
        public decimal Balance { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
