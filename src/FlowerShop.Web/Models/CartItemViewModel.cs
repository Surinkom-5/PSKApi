using FlowerShop.Core.Entities;
using System;

namespace FlowerShop.Web.Models
{
    public class CartItemViewModel
    {
        public int Quantity { get; private set; }
        public Guid CartId { get; private set; }
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