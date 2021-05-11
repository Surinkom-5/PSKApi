using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderId);

            builder.Property(x => x.PhoneNumber)
               .HasMaxLength(16)
               .IsRequired();

            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,4)")
               .IsRequired();

            builder.Property(x => x.OrderStatus)
               .HasDefaultValue(OrderStatus.Confirmed)
               .IsRequired();

            builder.HasMany(x => x.OrderItems)
               .WithOne(x => x.Order)
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
