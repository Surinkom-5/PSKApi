using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.AddressId);

            builder.Property(x => x.Street)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.City)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.PostalCode)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
