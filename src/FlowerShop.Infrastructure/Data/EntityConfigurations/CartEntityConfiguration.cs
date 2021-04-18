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
                .WithOne(x => x.Cart)
                .HasForeignKey<User>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Price)
                .IsRequired();

            builder.HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
