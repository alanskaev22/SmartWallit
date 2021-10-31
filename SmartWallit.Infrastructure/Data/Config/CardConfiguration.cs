using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Infrastructure.Data.Config
{
    class CardConfiguration : IEntityTypeConfiguration<CardEntity>
    {
        public void Configure(EntityTypeBuilder<CardEntity> builder)
        {
            builder.Property(c => c.DateCreated).HasDefaultValueSql("getdate()");
            builder.Property(c => c.CardNickname).HasMaxLength(25).IsRequired(false);

        }
    }

}
