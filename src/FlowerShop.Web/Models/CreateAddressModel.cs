using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Web.Models
{
    public class CreateAddressModel
    {
        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }
    }
}