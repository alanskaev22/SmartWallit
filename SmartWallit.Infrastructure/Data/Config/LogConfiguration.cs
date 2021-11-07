using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Infrastructure.Data.Config
{
    public class LogConfiguration : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder.Property(c => c.DateCreated).HasDefaultValueSql("getdate()");

        }
    }
}
