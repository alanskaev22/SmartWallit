using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using SmartWallit.Core.Entities;

namespace SmartWallit.Infrastructure.Data
{
    public class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options) : base(options)
        {
            //_provider = new AesProvider(_encryptionKey, _encryptionIV);
        }

        //private readonly byte[] _encryptionKey = Encoding.UTF8.GetBytes("SSljsdkkdlo4454Maakikjhsd55GaRTP");
        //private readonly byte[] _encryptionIV = Encoding.UTF8.GetBytes("SSljsdkkdlo4454Maakikjhsd55GaRTP");
        //private readonly IEncryptionProvider _provider;

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<WalletEntity> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>().ToTable("User");
            modelBuilder.Entity<CardEntity>().ToTable("Card");
            modelBuilder.Entity<WalletEntity>().ToTable("Wallet");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.UseEncryption(_provider);

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(u => u.PropertyType == typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }
    }
}
