using SmartWallit.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
