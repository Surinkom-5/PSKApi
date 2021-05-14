using System.Collections.Generic;

namespace FlowerShop.Web.Models
{
    public class AuthResultViewModel
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}