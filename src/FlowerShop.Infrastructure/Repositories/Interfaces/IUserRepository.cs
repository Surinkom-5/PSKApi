using FlowerShop.Core.Entities;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmailAsync(string email);
    }
}