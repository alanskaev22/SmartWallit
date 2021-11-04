using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Identity
{
    public class InitializeAppIdentityDbSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {

            if (!await userManager.Users.AnyAsync())
            {
                var user = new AppUser
                {
                    FirstName = "Alan",
                    LastName = "Skaev",
                    Email = "alan.skaev22@gmail.com",
                    UserName = "alan.skaev22",
                    Address = new Address
                    {
                        FirstName = "Alan",
                        LastName = "Skaev",
                        Street = "123 Main St.",
                        City = "Woodbridge",
                        State = "NJ",
                        ZipCode = "08861"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
