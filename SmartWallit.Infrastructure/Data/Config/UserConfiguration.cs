using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallit.Core.Entities;

namespace SmartWallit.Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DateOfBirth).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Password).IsRequired().HasMaxLength(25);
            builder.HasOne(u => u.Wallet).WithOne(w => w.User).HasForeignKey<WalletEntity>(w => w.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
