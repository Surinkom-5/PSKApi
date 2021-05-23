namespace FlowerShop.Infrastructure.Models
{
    public class CreateOrderResponse : BaseResponse
    {
        public int? OrderId { get; set; }

        public CreateOrderResponse(int orderId)
        {
            Success = true;
            OrderId = orderId;
        }

        public CreateOrderResponse(string error) : base(error)
        {
        }
    }
}
