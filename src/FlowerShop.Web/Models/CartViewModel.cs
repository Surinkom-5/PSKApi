using FlowerShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Web.Models
{
    public class CartViewModel
    {
        public Guid CartId { get; set; }
        public decimal Price { get; set; }
        public int? UserId { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }

        public static CartViewModel ToModel(Cart cart)
        {
            return new()
            {
                CartId = cart.Id,
                Price = cart.Price,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(CartItemViewModel.ToModel).ToList()
            };
        }
    }
}