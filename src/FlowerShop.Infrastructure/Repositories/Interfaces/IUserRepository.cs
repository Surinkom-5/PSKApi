using FlowerShop.Core.Entities;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmailAsync(string email);

        public Task<User> GetUserByIdAsync(int id);

        public Task<bool> CheckIfUserExists(string email);
    }
}