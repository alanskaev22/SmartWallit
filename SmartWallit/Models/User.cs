using SmartWallit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        internal DateTime _DateOfBirth { get; set; }
        public string DateOfBirth
        {
            get { return _DateOfBirth.ToShortDateString(); }
            set { _DateOfBirth = DateTime.Parse(value); }
        }

        public Wallet UserWallet { get; set; }
    }
}
