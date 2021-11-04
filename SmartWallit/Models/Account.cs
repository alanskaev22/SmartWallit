using SmartWallit.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class Account
    {
        public string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        internal DateTime? _DateOfBirth { get; set; }
        public string DateOfBirth
        {
            get { return _DateOfBirth?.ToShortDateString(); }
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
