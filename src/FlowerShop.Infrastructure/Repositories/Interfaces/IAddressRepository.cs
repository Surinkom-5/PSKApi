using FlowerShop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IAddressRepository
    {
        public Task<List<Address>> GetUserAddressesAsync(int userId);

        public Task<Address> GetAddressByIdAsync(int addressId);

        public Task<Address> GetUserAddressByIdAsync(int userId, int addressId);
    }
}