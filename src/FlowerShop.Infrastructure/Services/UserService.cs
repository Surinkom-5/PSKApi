using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public interface IUserService
    {
        public Task<int> AddUserAsync(string name, string email);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> AddUserAsync(string name, string email)
        {
            var newUser = new User(name, email);
            await _dbContext.AddAsync(newUser);

            await _dbContext.SaveChangesAsync();
            return newUser.UserId;
        }
    }
}