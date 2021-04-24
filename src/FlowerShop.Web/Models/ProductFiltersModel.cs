using FlowerShop.Core.Enums;

namespace FlowerShop.Web.Models
{
    public class ProductFiltersModel
    {
        public string SearchText { get; set; }
        public ProductType? ProductType { get; set; }
    }
}
