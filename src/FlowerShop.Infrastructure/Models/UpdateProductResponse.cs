namespace FlowerShop.Infrastructure.Models
{
    public class UpdateProductResponse : BaseResponse
    {
        public bool IsConcurrencyError { get; set; }

        public UpdateProductResponse()
        {
            Success = true;
        }

        public UpdateProductResponse(bool isConcurrencyError, string error) : base(error)
        {
            IsConcurrencyError = isConcurrencyError;
        }
    }
}
