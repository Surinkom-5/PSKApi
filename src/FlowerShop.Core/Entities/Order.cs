using FlowerShop.Core.Enums;
using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }
        public string PhoneNumber { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string Comment { get; private set; }
        public string Email { get; private set; } // TODO: Add migration
        public OrderStatus OrderStatus { get; private set; }
        public int? AddressId { get; private set; }
        public Address Address { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();


        private Order()
        {
        }

        public Order(string email, string phoneNumber, string comment, decimal totalPrice)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            Comment = comment;
            TotalPrice = totalPrice;
            OrderStatus = OrderStatus.InProgress;
        }
    }
}
