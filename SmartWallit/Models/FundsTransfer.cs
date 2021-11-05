using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class FundsTransfer
    {
        public int CardId { get; set; }
        public decimal Amount { get; set; }
    }
}
