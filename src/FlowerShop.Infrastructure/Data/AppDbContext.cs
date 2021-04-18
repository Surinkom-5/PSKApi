using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new AddressEntityConfiguration().Configure(modelBuilder.Entity<Address>());
            new CartEntityConfiguration().Configure(modelBuilder.Entity<Cart>());
            new CartItemEntityConfiguration().Configure(modelBuilder.Entity<CartItem>());
            new OrderEntityConfiguration().Configure(modelBuilder.Entity<Order>());
            new OrderItemEntityConfiguration().Configure(modelBuilder.Entity<OrderItem>());
            new ProductEntityConfiguration().Configure(modelBuilder.Entity<Product>());
            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
        }
    }
}