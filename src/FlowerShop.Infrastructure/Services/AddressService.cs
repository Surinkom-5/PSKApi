using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public interface IAddressService
    {
        public Task<int> AddNewAddressAsync(int userId, string street, string city, string postalCode);
        public Task<bool> RemoveAddressAsync(int addressId);
        public Task<bool> UpdateAddressAsync(int addressId, string street, string city, string postalCode);
    }

    public class AddressService : IAddressService
    {
        private readonly AppDbContext _dbContext;
        private readonly IAddressRepository _addressRepository;

        public AddressService(AppDbContext context, IAddressRepository addressRepository)
        {
            _dbContext = context;
            _addressRepository = addressRepository;
        }

        public async Task<int> AddNewAddressAsync(int userId, string street, string city, string postalCode)
        {
            var newAddress = new Address(userId, street, city, postalCode);
            await _dbContext.AddAsync(newAddress);

            await _dbContext.SaveChangesAsync();
            return newAddress.AddressId;
        }

        public async Task<bool> RemoveAddressAsync(int addressId)
        {
            var addressToRemove = await _addressRepository.GetAddressByIdAsync(addressId);

            if (addressToRemove == null)
            {
                return false;
            }

            _dbContext.Addresses.Remove(addressToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddressAsync(int addressId, string street, string city, string postalCode)
        {
            var addressToUpdate = await _addressRepository.GetAddressByIdAsync(addressId);

            if (addressToUpdate == null)
            {
                return false;
            }

            addressToUpdate.ChangeAddress(street, city, postalCode);
            _dbContext.Entry(addressToUpdate).CurrentValues.SetValues(addressToUpdate);

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
