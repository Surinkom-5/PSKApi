using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;

namespace FlowerShop.Infrastructure.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        public Task<Cart> GetCartByIdAsync(int id);
    }
}
