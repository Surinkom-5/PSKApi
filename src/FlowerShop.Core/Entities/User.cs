using FlowerShop.Core.Enums;
using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class User
    {
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserRole UserRole { get; private set; }

        private readonly List<Address> _addresses = new();
        public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

        public Cart Cart { get; private set;}

        private readonly List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        private User() 
        {
        }

    }
}
