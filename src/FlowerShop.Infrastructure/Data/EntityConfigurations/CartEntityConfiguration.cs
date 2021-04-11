using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.CartId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.Bouquets)
                .WithMany(f => f.Carts);

            builder.HasMany(f => f.Flowers)
                .WithMany(f => f.Carts);
        }
    }
}
