using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class Card
    {
        public int Id { get; private set; }
        [Required, StringLength(19, MinimumLength = 8)]
        public string CardNumber { get; set; }

        [Required, RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Expiration Year must be YYYY format")]
        public int ExpirationYear { get; set; }
        [Required, RegularExpression(@"^[0-9]{2}$", ErrorMessage = "Expiration Month must be MM format")]
        public int ExpirationMonth { get; set; }
        public string CardNickname { get; set; }
    }
}
