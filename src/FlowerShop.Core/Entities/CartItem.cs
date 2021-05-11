using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Core.Entities
{
    public class CartItem
    {
        public int Quantity { get; private set; }
        public Guid CartId { get; private set; }
        public Cart Cart { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        private CartItem()
        {
        }

        public CartItem(Guid cartId, int productId, int quantity)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }

        public static List<OrderItem> ToOrderItems(List<CartItem> cartItems, int orderId)
        {
            return cartItems.Select(i => new OrderItem(i.Quantity, orderId, i.ProductId)).ToList();
        }
    }
}
