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
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.User).IsRequired();
            builder.HasMany(w => w.Cards).WithOne().HasForeignKey(c => c.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
