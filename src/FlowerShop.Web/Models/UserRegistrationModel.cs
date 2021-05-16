using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Web.Models
{
    public class UserRegistrationModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}