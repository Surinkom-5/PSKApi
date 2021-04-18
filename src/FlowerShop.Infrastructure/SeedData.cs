using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FlowerShop.Infrastructure
{
    public static class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (dbContext.Products.Any())
            {
                return;   // DB has been seeded
            }

            PopulateTestData(dbContext);
        }

        public static void PopulateTestData(AppDbContext dbContext)
        {
            foreach (var item in dbContext.Products)
            {
                dbContext.Remove(item);
            }

            SeedProducts(dbContext);
        }

        private static void SeedProducts(AppDbContext dbContext)
        {
            var product1 = new Product("Rose", 9.99m, "A rose is a woody perennial flowering plant of the genus Rosa, in the family Rosaceae, or the flower it bears.",
                ProductType.Flower);
            var product2 = new Product("Lily", 9.99m, "Lilium (members of which are true lilies) is a genus of herbaceous flowering plants growing from bulbs, all with large prominent flowers.",
                ProductType.Flower);
            var product3 = new Product("Lavender bouquet", 19.99m, "Lavandula (common name lavender) is a genus of 47 known species of flowering plants in the mint family, Lamiaceae.",
                ProductType.Bouquet);
            var product4 = new Product("Albuca spiralis", 13.99m, "Albuca spiralis, commonly called the corkscrew albuca, is a species of flowering plant in the family Asparagaceae, that is native to Western and Northern Cape Provinces, South Africa.",
                ProductType.PotterPlant);

            product1.SetAvailabilityCount(10);
            product3.SetAvailabilityCount(5);
            product4.SetAvailabilityCount(5);

            dbContext.Products.Add(product1);
            dbContext.Products.Add(product2);
            dbContext.Products.Add(product3);
            dbContext.Products.Add(product4);

            dbContext.SaveChanges();
        }
    }
}
