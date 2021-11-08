using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartWallit.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressFromClaims(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims.FirstOrDefault(c => c.Type == "userEmail").Value;

            return await input.Users.Include(u => u.Address).SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaims(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims.FirstOrDefault(c => c.Type == "userEmail").Value;

            return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
