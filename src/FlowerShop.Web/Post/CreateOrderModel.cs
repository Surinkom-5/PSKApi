using System;

namespace FlowerShop.Web.Post
{
    public class CreateOrderModel
    {
        public int? AddressId { get; set; } // If order is created by registered user
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }

        public Infrastructure.CustomModels.CreateOrderModel ToCreateOrderModel(int? userId, Guid cartId = default)
        {
            return new Infrastructure.CustomModels.CreateOrderModel()
            {
                UserId = userId,
                AddressId = AddressId,
                Email = Email,
                Comment = Comment,
                CartId = cartId,
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                City = City,
                PostCode = PostCode,
                PhoneNumber = PhoneNumber
            };
        }
    }
}