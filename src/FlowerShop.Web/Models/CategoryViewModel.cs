using FlowerShop.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System;
using FlowerShop.Core.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlowerShop.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public static CategoryViewModel ToModel(ProductType productType)
        {
            return new CategoryViewModel
            {
                Id = (int)productType,
                Name = ProductTypeTranslator(productType),
                Image = ProductImageByType(productType)
            };
        }

        public static List<CategoryViewModel> ToModel(List<ProductType> productTypes)
        {
            return (productTypes.Select(productType => ToModel(productType))).ToList();
        }

        private static string ProductTypeTranslator(ProductType productType)
        {
            var map = new Dictionary<int, string>()
            {
                {0, "Puokštė"},
                {1, "Gėlė"},
                {2, "Gėlės vazonuose"}
            };
            return map.TryGetValue((int)productType, out string output) ? output : "Nėra aprašymo";
        }

        private static string ProductImageByType(ProductType productType)
        {
            var map = new Dictionary<int, string>()
            {
                {0, "https://res.cloudinary.com/dzpfau9nh/image/upload/v1621696628/bouquet_3.jpg"},
                {1, "https://res.cloudinary.com/dzpfau9nh/image/upload/v1621696629/tulip.jpg"},
                {2, "https://res.cloudinary.com/dzpfau9nh/image/upload/v1621697499/begonia_g0y463.jpg"}
            };
            return map.TryGetValue((int)productType, out string output) ? output : "";
        }
    }
}