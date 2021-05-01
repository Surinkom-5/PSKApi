namespace FlowerShop.Core.Entities
{
    public class CartItem
    {
        public int Quantity { get; private set; }
        public int CartId { get; private set; }
        public Cart Cart { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        private CartItem()
        {
        }

    }
}
