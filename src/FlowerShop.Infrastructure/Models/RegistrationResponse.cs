using System.Collections.Generic;

namespace FlowerShop.Infrastructure.Models
{
    public class RegistrationResponse
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}