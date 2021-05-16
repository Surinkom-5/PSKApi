using FlowerShop.Core.Entities;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        public Task<Cart> GetCartByPublicIdAsync(string id);

        public Task<Cart> GetCartByUserId(int id);
    }
}