using FlowerShop.Core.Entities;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<Cart> CreateCart();

        public Task<bool> AddItemToCart(string cartId, int itemId, int quantity);

        public Task<bool> RemoveItemFromCart(string cartId, int itemId);
    }
}