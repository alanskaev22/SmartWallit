using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class ChangePasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\w!@#$%^&*?~()-]{8,}$", ErrorMessage = "Password must contain at least 1 upper-case letter, 1 special character, 1 digit, and be longer than six charaters.")]
        public string NewPassword { get; set; }
    }
}
