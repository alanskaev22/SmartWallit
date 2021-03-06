using SmartWallit.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class RegisterRequest
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\w!@#$%^&*?~()-]{8,}$", ErrorMessage = "Password must contain at least 1 upper-case letter, 1 special character, 1 digit, and be longer than six charaters.")]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [JsonIgnore]
        internal DateTime _DateOfBirth { get; set; }
        [Required]         //DataType(DataType.DateTime, ErrorMessage = "Date must be in YYY-MM-DD format")
        public string DateOfBirth
        {
            get { return _DateOfBirth.ToShortDateString(); }
            set
            {
                try
                {
                    _DateOfBirth = DateTime.Parse(value);
                }
                catch
                {
                    throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Date must be in YYY-MM-DD format", nameof(DateOfBirth));
                }
            }
        }

        public AddressModel Address { get; set; }
    }
}
