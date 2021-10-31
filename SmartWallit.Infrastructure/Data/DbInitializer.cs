using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(WalletContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
            {
                var user = await context.Users.FirstOrDefaultAsync();   

                return; // DB has been seeded
            }

            var users = new UserEntity[]
            {
            new UserEntity{FirstName="Alan",LastName="Skaev",DateOfBirth=DateTime.Parse("1995-07-19"), Email = "alan.skaev22@gmail.com", Password="password"},
            };
            foreach (UserEntity u in users)
            {
                await context.Users.AddAsync(u);
            }
            
            await context.SaveChangesAsync();
        }
    }
}
