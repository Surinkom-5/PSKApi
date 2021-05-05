using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _dbContext;

        public AddressRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Address>> GetUserAddressesAsync(int userId)
        {
            return await _dbContext.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.AddressId == addressId);
        }
    }
}
