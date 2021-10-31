using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallit.Core.Entities;
using System;

namespace SmartWallit.Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u => u.Id).IsRequired();
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.DateOfBirth).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Password).IsRequired().HasMaxLength(25);
            builder.Property(u => u.DateCreated).HasDefaultValueSql("getdate()");
            builder.HasOne(u => u.Wallet)
                .WithOne()
                .HasForeignKey<WalletEntity>(w => w.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
