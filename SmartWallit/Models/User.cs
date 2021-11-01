using SmartWallit.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class User
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [JsonIgnore]
        internal DateTime _DateOfBirth { get; set; }
        [Required, DataType(DataType.DateTime)]
        public string DateOfBirth
        {
            get { return _DateOfBirth.ToShortDateString(); }
            set { _DateOfBirth = DateTime.Parse(value); }
        }

    }
}
