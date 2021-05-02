using FlowerShop.Core.Entities;

namespace FlowerShop.Web.Models
{
    public class CartItemViewModel
    {
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
