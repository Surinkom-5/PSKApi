using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.CustomModels
{
    public class CreateOrderModel
    {
        public int? UserId { get; set; } // If order is created by registered user
        public int? AddressId { get; set; } // If order is created by registered user
        public Guid CartId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Comment { get; set; }
    }
}
