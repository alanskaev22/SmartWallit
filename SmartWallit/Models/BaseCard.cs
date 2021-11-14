using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Models
{
    public class BaseCard
    {
        public int Id { get; private set; }
        [Required, RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Expiration Year must be YYYY format.")]
        public int ExpirationYear { get; set; }
        [Required, RegularExpression(@"^([1-9]|1[0-2])$", ErrorMessage = "Expiration Month must be between 1 and 12.")]
        public int ExpirationMonth { get; set; }
        [StringLength(50)]
        public string CardNickname { get; set; }
        public string CardBrand { get; private set; }
    }
}
