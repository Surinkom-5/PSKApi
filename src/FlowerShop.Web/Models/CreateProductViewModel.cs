using FlowerShop.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Web.Models
{
    public class CreateProductViewModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(512)]
        public string Description { get; set; }
        [Required]
        public ProductType ProductType { get; set; }
        public int AvailabilityCount { get; set; }
    }
}
