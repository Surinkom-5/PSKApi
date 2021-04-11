using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<Bouquet> Bouquets { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FlowersBouquet> FlowersBouquets { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new FlowerEntityConfiguration().Configure(modelBuilder.Entity<Flower>());
            new BouquetEntityConfiguration().Configure(modelBuilder.Entity<Bouquet>());
            new CartEntityConfiguration().Configure(modelBuilder.Entity<Cart>());
            new FlowersBouquetEntityConfiguration().Configure(modelBuilder.Entity<FlowersBouquet>());
            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
        }
    }
}