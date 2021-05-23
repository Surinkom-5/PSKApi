﻿using System;

namespace FlowerShop.Web.Patch
{
    public class ProductPatch
    {
        public int? Quantity { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public byte[] Version { get; set; }
    }
}