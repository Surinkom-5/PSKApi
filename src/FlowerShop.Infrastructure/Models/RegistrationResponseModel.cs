using System.Collections.Generic;

namespace FlowerShop.Infrastructure.Models
{
    public class RegistrationResponseModel
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}