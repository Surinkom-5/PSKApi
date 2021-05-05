using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IUserRepository
    {
        public Task<int> GetFirstUserIdAsync();
    }
}
