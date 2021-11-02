using SmartWallit.Core.Exceptions;
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
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [JsonIgnore]
        internal DateTime _DateOfBirth { get; set; }
        [Required, DataType(DataType.DateTime, ErrorMessage = "Date must be in YYY-MM-DD format" )]
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

    }
}
