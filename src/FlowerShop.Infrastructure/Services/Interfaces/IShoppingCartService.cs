using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<Cart> CreateCart();

        public Task<bool> AddItemToCart(string cartId, int itemId, int quantity);

        public Task<bool> RemoveItemFromCart(string cartId, int itemId);
    }
}
