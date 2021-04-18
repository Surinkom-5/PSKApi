﻿using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Name)
               .HasMaxLength(64)
               .IsRequired();

            builder.Property(x => x.Price)
               .IsRequired();

            builder.Property(x => x.Description)
               .HasMaxLength(512)
               .IsRequired();

            builder.Property(x => x.AvailabilityCount)
               .HasDefaultValue(0);
        }
    }
}
