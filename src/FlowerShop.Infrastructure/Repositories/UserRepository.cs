using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<bool> CheckIfUserExists(string email)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email) != null;
        }
    }
}