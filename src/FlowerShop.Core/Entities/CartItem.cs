namespace FlowerShop.Core.Entities
{
    public class CartItem
    {
        public int CartItemId { get; private set; }
        public int Quantity { get; private set; }
        public int CartId { get; private set; }
        public Cart Cart { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        private CartItem()
        {
        }

        public CartItem(int cartId, int productId, int quantity)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }

    }
}
