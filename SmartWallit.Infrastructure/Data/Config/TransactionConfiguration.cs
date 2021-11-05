using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Infrastructure.Data.Config
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.Property(c => c.DateCreated).HasDefaultValueSql("getdate()");
        }
    }
}
