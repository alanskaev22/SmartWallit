using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallit.Core.Entities;

namespace SmartWallit.Infrastructure.Data.Config
{
    public class WalletConfiguration : IEntityTypeConfiguration<WalletEntity>
    {
        public void Configure(EntityTypeBuilder<WalletEntity> builder)
        {
            builder.Property(w => w.Balance).HasColumnType("decimal(18,2)");
            builder.Property(w => w.DateCreated).HasDefaultValueSql("getdate()");
            builder.HasIndex(w => w.UserId).IsUnique();
        }
    }
}
