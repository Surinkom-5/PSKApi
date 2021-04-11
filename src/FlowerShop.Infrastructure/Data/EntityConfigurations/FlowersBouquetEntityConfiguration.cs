using FlowerShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.Infrastructure.Data.Config
{
    public class FlowersBouquetEntityConfiguration : IEntityTypeConfiguration<FlowersBouquet>
    {
        public void Configure(EntityTypeBuilder<FlowersBouquet> builder)
        {
            builder.HasKey(x => new { x.BouquetId, x.FlowerId });

            builder.Property(x => x.FlowerCount)
                .IsRequired();

            builder.HasOne(x => x.Flower)
                .WithMany(x => x.FlowersBouquet)
                .HasForeignKey(x => x.FlowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Bouquet)
                .WithMany(x => x.FlowersBouquet)
                .HasForeignKey(x => x.BouquetId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
