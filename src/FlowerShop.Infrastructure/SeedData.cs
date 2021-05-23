using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using FlowerShop.Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace FlowerShop.Infrastructure
{
    public static class SeedData
    {
        private static string _imageBaseUrl;

        private static readonly List<string> ImageUrls = new ()
        {
            "v1621696628/rose.jpg",
            "v1621696628/lilly.jpg",
            "v1621696629/tulip.jpg",
            "v1621696629/white_rose.jpg",
            "v1621695939/sunflower.webp",

            "v1621696628/bouquet_1.jpg",
            "v1621011506/bouquet_2.jpg",
            "v1621696628/bouquet_3.jpg",
            "v1621696628/bouquet_4.jpg",

            "v1621697499/cactus_small.jpg",
            "v1621697499/cactus_medium.jpg",
            "v1621697520/papartis.jpg",
            "v1621697499/begonia_g0y463.jpg",
            "v1621697499/katil%C4%97lis_u1gica.jpg",
            "v1621697499/raktazole_wnwjvk.jpg",
            "v1621697499/kalanke.jpg"
        };

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            var imageUrlBase = serviceProvider.GetService<ImageConfig>();
            _imageBaseUrl = imageUrlBase.ImageBaseUrl;

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
                // Flowers
                new Product("Rožė", 9.99m, "Rožės dažnai laikomos dieviškumo, tobulo grožio, o raudonosios – meilės simboliu. Vakarų pasaulyje rožė laikoma kilmingiausia gėle. Jos atskirai ar puokštėse dovanojamos įvairiomis progomis.",
                    ProductType.Flower),
                new Product("Lelija", 5.49m, "lelijinių (Liliaceae) šeimos augalų gentis, kuriai priklauso daugiametės svogūninės žolės, turinčios kiaušiniškus svogūnus ir stačius lapuotus stiebus. Lapai dažniausiai siauri, bekočiai. Žiedai įvairių spalvų ir dydžio, vienų rūšių nusvirę, kitų statūs, sukrauti kekėse ar išaugę pavieniui.",
                    ProductType.Flower),
                new Product("Tulip ", 4.99m, "lelijinių augalų (Liliaceae) gentis. Joms būdingi dideli šešių žiedlapių žiedai. Iš viso yra apie 100 rūšių, Lietuvoje auginamos tik kaip dekoratyviniai augalai. Tulpės kilusios iš pietų Europos, Šiaurės Afrikos ir Azijos ",
                    ProductType.Flower),
                new Product("Baltoji rožė", 9.99m, "Rožės dažnai laikomos dieviškumo, tobulo grožio, o raudonosios – meilės simboliu. Vakarų pasaulyje rožė laikoma kilmingiausia gėle. Jos atskirai ar puokštėse dovanojamos įvairiomis progomis.", 
                    ProductType.Flower),
                new Product("Saulėgraža", 7.99m, "dideli žiedynai atrodo kaip saulė; be to, jie dažniausiai būna pasisukę į saulės pusę.", 
                    ProductType.Flower),

                // Bouquets
                new Product("Puokštė \"Pavasarinis grožis\"", 35.00m, "Puokštė jai arba jam. Tobulai tinkanti sodraus vėlyvo pavasario progoms.",
                    ProductType.Bouquet),
                new Product("Puokštė \"Alpių rasa\"", 25.00m, "Puokštė, kupina gaivių prisiminimų.",
                    ProductType.Bouquet),
                new Product("Puokštė \"Pavasario rytas\"", 39.00m, "Nuostabi puokštė jūsų artimųjų rytą pavers saulėtu ir gražiu.",
                    ProductType.Bouquet),
                new Product("Puokštė \"Švelnūs jausmai\"", 48.00m, "Puokštės sudėtis: rožės, eustomos, eukaliptas.",
                    ProductType.Bouquet),

                // Potter plants
                new Product("Miniatiūrinis kaktusas vazone (mažas)", 7.99m, "Kaktusai – tai augalai, prisitaikę augti pačiomis nepalankiausiomis sąlygomis, kai trūksta drėgmės, kai pastovios aukštos temperatūros ir kepina saulė. Dažniausiai – tai dykvietės, smėlynai, dykumos ir kitos nepalankios augimvietės. Todėl kaktusus reikia auginti ypač skurdžioje vandeniui pralaidžioje smėlėtoje dirvoje su geru drenažu. Laikyti ypač šviesioje, saulėtoje, šiltoje vietoje. tik tuomet augalas bus kompaktiškas ir gražus, įgaus jam būdingą spalvą. Tamsoje augalai tįsta ir deformuojasi. Laistyti ir tręšti ypač saikingai.  Vasarą laistyti kartą ar du per mėnesį, galima ir rečiau. Tręšti ne dažniau kaip 1 kartą per mėnesį ar du. Žiemą, kai tamsu ir augalas yra ramybės stadijoje, laistyti dar rečiau. Darykite poros mėnesių pertraukas. Neperlaistykite augalo, nepakenčia užmirkimo. Kai per daug drėgmės, pūna augalo šaknys. Vasarą puikiai jaučiasi balkone, terasoje gryname ore.",
                    ProductType.PotterPlant),
                new Product("Miniatiūrinis kaktusas vazone (vidutinis)", 15.49m, "Kaktusai – tai augalai, prisitaikę augti pačiomis nepalankiausiomis sąlygomis, kai trūksta drėgmės, kai pastovios aukštos temperatūros ir kepina saulė. Dažniausiai – tai dykvietės, smėlynai, dykumos ir kitos nepalankios augimvietės. Todėl kaktusus reikia auginti ypač skurdžioje vandeniui pralaidžioje smėlėtoje dirvoje su geru drenažu. Laikyti ypač šviesioje, saulėtoje, šiltoje vietoje. tik tuomet augalas bus kompaktiškas ir gražus, įgaus jam būdingą spalvą. Tamsoje augalai tįsta ir deformuojasi. Laistyti ir tręšti ypač saikingai.  Vasarą laistyti kartą ar du per mėnesį, galima ir rečiau. Tręšti ne dažniau kaip 1 kartą per mėnesį ar du. Žiemą, kai tamsu ir augalas yra ramybės stadijoje, laistyti dar rečiau. Darykite poros mėnesių pertraukas. Neperlaistykite augalo, nepakenčia užmirkimo. Kai per daug drėgmės, pūna augalo šaknys. Vasarą puikiai jaučiasi balkone, terasoje gryname ore.",
                    ProductType.PotterPlant),
                new Product("Papartis", 3.50m, "Inkstpapartis (Nephrolepis). Turi plunksniškus, karpytus lapus, ilgus šakniastiebius. Kilęs iš paatogrąžių, laisvai auga drėgnuose miškuose. Laikymas: pietryčių ir pietvakarių pusė. Vasarą galima laikyti lauke. \nApšvietimas: vasarą reikalinga labiau pavėsinga vieta.",
                    ProductType.PotterPlant),
                new Product("Begonija", 8.49m, "Priklauso begoninių šeimai, natūraliai auga Azijos ir Amerikos atogrąžų miškuose. Augalų gentis išsiskiria asimetriškais lapais, gali būti įvairių formų ir spalvų. Žiedai būna tuščiaviduriai arba pilnaviduriai, įvairių spalvų.\r\nLaikymas: parinkti nesaulėtą, gerai vėdinamą kambario vietą. Rytinė arba vakarinė palangė.\r\nApšvietimas: saugoti nuo tiesioginių saulės spindulių. Jie gali nudeginti augalą.\r\nTemperatūra: 20–25 °C.\r\nLaistymas: vasarą gausus, žiemą – rečiau. Vanduo turi būti minkštas.\r\nRamybės laikotarpis: pavasarį reikalinga vėsesnė patalpa ir retesnis laistymas.",
                    ProductType.PotterPlant),
                new Product("Katilėlis", 4.49m, "Natūraliai auga Viduržemio jūros regione, priklauso katilėlinių augalų grupei. Auga besidriekiančiu krūmeliu 15 - 20 cm aukščio. Daugiametis žolinis augalas, turi varpelio formos baltus ar mėlynus žiedus. Auginamas pastatomuose arba pakabinamuose vazonuose.\r\nLaikymas: rytinėje arba pietinėje pusėje. Vasarą auginama lauke.\r\nApšvietimas: šviesi arba labai ryški vieta. Jeigu trūksta šviesos, augalas pradeda skursti.\r\nTemperatūra: 20–25 °C.",
                    ProductType.PotterPlant),
                new Product("Raktažolė", 3.49m, "Šviesiai žalios spalvos lapai, raukšlėti. Žiedai smulkūs, kvepiantys, pilnaviduriai arba tuščiaviduriai gali būti įvairiausių spalvų. Dažniausiai auginama kaip vienmetė, tačiau galima auginti ir visus metus.\r\nLaikymas: šiaurinė palangė.\r\nApšvietimas: saugoti nuo tiesioginių saulės spindulių. Jie gali nudeginti augalą. Reikia išsklaidytos šviesos, jeigu pradeda trūkti jos, augalas tįsta ir pradeda anksčiau žydėti, prastesniais žiedais.\r\nTemperatūra: 10–15°C.",
                    ProductType.PotterPlant),
                new Product("Kalankė", 6.99m, "Sukulentinė, ypač ilgai žydinti gėlė, kuri gali džiuginti įvairiaspalviais žiedais visus metus. Kilusi iš Madagaskaro, puikiai auga ir mūsų namų sąlygomis.\r\nLaikymas: vakarinė ir rytinė palangė. Žiemą perkelti ant pietinės.\r\nApšvietimas: mėgsta šviesią vietą, tačiau nepakenčia tiesioginių saulės spindulių.\r\nTemperatūra: 12–30 °C.",
                    ProductType.PotterPlant),
            };

            products.ForEach(x => x.SetAvailabilityCount(5));
            products.Last().SetAvailabilityCount(0);

            for (var i = 0; i < products.Count; i++)
            {
                products[i].SetImageUrl(_imageBaseUrl + ImageUrls[i]);
            }

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