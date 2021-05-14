using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new AddressEntityConfiguration().Configure(builder.Entity<Address>());
            new CartEntityConfiguration().Configure(builder.Entity<Cart>());
            new CartItemEntityConfiguration().Configure(builder.Entity<CartItem>());
            new OrderEntityConfiguration().Configure(builder.Entity<Order>());
            new OrderItemEntityConfiguration().Configure(builder.Entity<OrderItem>());
            new ProductEntityConfiguration().Configure(builder.Entity<Product>());
            new UserEntityConfiguration().Configure(builder.Entity<User>());
        }
    }
}