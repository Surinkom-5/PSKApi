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
    }
}
