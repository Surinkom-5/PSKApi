using FlowerShop.Core.Entities;
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

            if (dbContext.Flowers.Any())
            {
                return;   // DB has been seeded
            }

            PopulateTestData(dbContext);
        }
        public static void PopulateTestData(AppDbContext dbContext)
        {
            foreach (var item in dbContext.Flowers)
            {
                dbContext.Remove(item);
            }

            var flowerItem = new Flower("Rose");
            var flowerItem2 = new Flower("Lily");
            var flowerItem3 = new Flower("Tulip");

            dbContext.SaveChanges();
            dbContext.Flowers.Add(flowerItem);
            dbContext.Flowers.Add(flowerItem2);
            dbContext.Flowers.Add(flowerItem3);

            dbContext.SaveChanges();
        }
    }
}
