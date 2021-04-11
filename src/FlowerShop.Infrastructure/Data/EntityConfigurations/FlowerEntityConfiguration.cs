using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class FlowerEntityConfiguration : IEntityTypeConfiguration<Flower>
    {
        public void Configure(EntityTypeBuilder<Flower> builder)
        {
            builder.HasKey(f => f.FlowerId);

            builder.Property(f => f.Flowername)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(f => f.Price)
                .IsRequired();

            builder.Property(f => f.FlowersInStorage);
        }
    }
}
