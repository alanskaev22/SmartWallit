using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public ICollection<CardResponse> Cards { get; set; }
    }
}
