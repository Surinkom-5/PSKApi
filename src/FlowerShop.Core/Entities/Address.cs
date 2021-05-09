namespace FlowerShop.Core.Entities
{
    public class Address
    {
        public int AddressId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }

        private Address()
        {
        }

        public Address(int userId, string street, string city, string postalCode)
        {
            UserId = userId;
            Street = street;
            City = city;
            PostalCode = postalCode;
        }

        public void ChangeAddress(string street, string city, string postalCode)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
        }
    }
}