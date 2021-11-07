using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Models
{
    public class CardResponse : BaseCard
    {
        [Required, StringLength(19, MinimumLength = 8)]
        public string CardNumber { get; set; }
    }
}
