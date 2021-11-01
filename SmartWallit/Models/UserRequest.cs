using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWallit.Models
{
    public class UserRequest : User
    {
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
    }
}
