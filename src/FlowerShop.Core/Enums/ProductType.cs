using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Core.Enums
{
    public enum ProductType
    {
        [Display(Name = "Gėlių puokštė")]
        Bouquet = 0,
        [Display(Name = "Gėlė")]
        Flower = 1,
        [Display(Name = "Vazoninis augalas")]
        PotterPlant = 2
    }
}