using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class CardRequest : CardResponse
    {
        [Required, RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Cvv must be 3 or 4 characters")]
        public int Cvv { get; set; }
    }
}
