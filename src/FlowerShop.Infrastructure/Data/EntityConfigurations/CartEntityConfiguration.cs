using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}