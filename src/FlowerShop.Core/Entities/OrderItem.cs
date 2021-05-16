namespace FlowerShop.Core.Entities
{
    public class OrderItem
    {
        public int Quantity { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        private OrderItem()
        {
        }

        public OrderItem(int quantity, int orderId, int productId)
        {
            Quantity = quantity;
            OrderId = orderId;
            ProductId = productId;
        }
    }
}