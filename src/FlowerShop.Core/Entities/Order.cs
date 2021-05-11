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
        public string Email { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public int? AddressId { get; private set; }
        public Address Address { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }

        // Creation fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressValue { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }


        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();


        private Order()
        {
        }

        public Order(string email, 
            string phoneNumber, 
            string comment, 
            decimal totalPrice, 
            string firstName, 
            string lastName, 
            string addressValue, 
            string city, 
            string postCode)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            Comment = comment;
            TotalPrice = totalPrice;
            OrderStatus = OrderStatus.Confirmed;
            FirstName = firstName;
            LastName = lastName;
            AddressValue = addressValue;
            City = city;
            PostCode = postCode;
        }
    }
}
