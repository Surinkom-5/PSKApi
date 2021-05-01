using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => new { x.CartId, x.ProductId});

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.HasOne(x => x.Cart)
                .WithMany(x => x.CartItems)
                .HasPrincipalKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
