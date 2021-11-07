using System.ComponentModel.DataAnnotations;

namespace SmartWallit.Models
{
    public class UpdateCardRequest : BaseCard
    {
        [Required]
        public new int Id { get; set; }
        [Required, RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Cvv must be 3 or 4 characters")]
        public int Cvv { get; set; }
    }
}
