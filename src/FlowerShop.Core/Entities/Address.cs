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

    }
}
