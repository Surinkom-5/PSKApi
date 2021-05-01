using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;

namespace FlowerShop.Web.Models
{
    public class CartItemViewModel
    {
        public int CartItemId { get; private set; }
        public int Quantity { get; private set; }
        public int CartId { get; private set; }
        public int ProductId { get; private set; }

        public static CartItemViewModel ToModel(CartItem item)
        {
            return new CartItemViewModel()
            {
                Quantity = item.Quantity,
                CartId = item.CartId,
                ProductId = item.ProductId
            };
        }
    }
}
