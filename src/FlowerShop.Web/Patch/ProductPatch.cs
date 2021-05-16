using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Web.Patch
{
    public class ProductPatch
    {
        public int? Quantity { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }
        public decimal? Price { get; set; }
    }
}
