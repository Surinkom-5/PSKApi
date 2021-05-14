using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            SeedDatabase(dbContext);
        }

        public static void SeedDatabase(AppDbContext dbContext)
        {
            foreach (var item in dbContext.Products)
            {
                dbContext.Remove(item);
            }

            SeedProducts(dbContext);
            SeedRoles(dbContext);
            SeedUsers(dbContext);
            SeedAddresses(dbContext);
            CreateFullTextCatalog(dbContext);
        }

        private static void SeedProducts(AppDbContext dbContext)
        {
            if (dbContext.Products.Any())
            {
                return;   // DB has been seeded
            }
            var products = new List<Product>
            {
                new Product("Rose", 9.99m, "A rose is a woody perennial flowering plant of the genus Rosa, in the family Rosaceae, or the flower it bears.",
                    ProductType.Flower),
                new Product("Lily", 9.99m, "Lilium (members of which are true lilies) is a genus of herbaceous flowering plants growing from bulbs, all with large prominent flowers.",
                    ProductType.Flower),
                new Product("Lavender bouquet", 19.99m, "Lavandula (common name lavender) is a genus of 47 known species of flowering plants in the mint family, Lamiaceae.",
                    ProductType.Bouquet),
                new Product("Albuca spiralis", 13.99m, "Albuca spiralis, commonly called the corkscrew albuca, is a species of flowering plant in the family Asparagaceae, that is native to Western and Northern Cape Provinces, South Africa.",
                    ProductType.PotterPlant),
                new Product("Tulip ", 4.99m, "Tulips (Tulipa) form a genus of spring-blooming perennial herbaceous bulbiferous geophytes (having bulbs as storage organs).",
                    ProductType.Flower),
                new Product("Bouquet for You", 29.99m, "These flowers are sure to bring one good energy and joy! Nice and beautiful bouquet of gerbera for every occasion.",
                    ProductType.Bouquet),
            };

            products.ForEach(x => x.SetAvailabilityCount(5));
            products.Last().SetAvailabilityCount(0);

            dbContext.Products.AddRange(products);

            dbContext.SaveChanges();
        }

        private static void SeedUsers(AppDbContext dbContext)
        {
            if (dbContext.ApplicationUsers.Any())
            {
                return;   // DB has been seeded
            }

            var ownerRoleId = dbContext.Roles.FirstOrDefault(a => a.Name.Equals("Owner")).Id;

            var userIdentity = new IdentityUser("user@mail.com") { Email = "user@mail.com", NormalizedEmail = "USER@MAIL.COM" };
            var ownerIdentity = new IdentityUser("owner@mail.com") { Email = "owner@mail.com", NormalizedEmail = "OWNER@MAIL.COM" };

            var ph = new PasswordHasher<IdentityUser>();
            ownerIdentity.PasswordHash = ph.HashPassword(ownerIdentity, "admin");
            userIdentity.PasswordHash = ph.HashPassword(ownerIdentity, "user");
            dbContext.Users.Add(userIdentity);
            dbContext.Users.Add(ownerIdentity);
            dbContext.SaveChanges();

            dbContext.UserRoles.Add(new IdentityUserRole<string>() { UserId = ownerIdentity.Id, RoleId = ownerRoleId.ToString() });
            var user = new User("user", "user@mail.com");
            var ownerAppUser = new User("Admin admin", "owner@mail.com");

            ownerAppUser.SetUserRole(UserRole.Owner);

            dbContext.ApplicationUsers.Add(user);
            dbContext.ApplicationUsers.Add(ownerAppUser);

            dbContext.SaveChanges();
        }

        private static void SeedRoles(AppDbContext dbContext)
        {
            if (dbContext.Roles.Any())
            {
                return;   // DB has been seeded
            }

            dbContext.Roles.Add(new IdentityRole
            {
                Name = "Owner",
                NormalizedName = "OWNER"
            });

            dbContext.SaveChanges();
        }

        private static void SeedAddresses(AppDbContext dbContext)
        {
            if (dbContext.Addresses.Any())
            {
                return;   // DB has been seeded
            }

            var users = dbContext.ApplicationUsers.ToList();
            var addresses = new List<Address>();
            users.ForEach(u => addresses.Add(new Address(u.UserId, "testStreet", "testCity", "testCode")));

            dbContext.Addresses.AddRange(addresses);

            dbContext.SaveChanges();
        }

        private static void CreateFullTextCatalog(AppDbContext dbContext)
        {
            const string sql = "IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE[name] = 'Search')" +
                " BEGIN" +
                "   CREATE FULLTEXT CATALOG Search WITH ACCENT_SENSITIVITY = OFF" +
                "   CREATE FULLTEXT INDEX ON dbo.Products(name) KEY INDEX PK_Products ON Search" +
                " END";
            dbContext.Database.ExecuteSqlRawAsync(sql);

            dbContext.SaveChanges();
        }
    }
}