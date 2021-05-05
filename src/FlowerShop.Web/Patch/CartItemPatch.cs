using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Web.Patch
{
    public class CartItemPatch
    {
        public int productId { get; set; }
        public int quantity { get; set; }
    }
}
