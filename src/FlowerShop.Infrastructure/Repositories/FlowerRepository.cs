using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IFlowerRepository
    {
        public Task<Flower> GetFlowerById(int flowerId);
    }

    public class FlowerRepository : IFlowerRepository
    {
        private readonly AppDbContext _dbContext;

        public FlowerRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Flower> GetFlowerById(int flowerId)
        {
            return await _dbContext.Flowers.FirstOrDefaultAsync(f => f.FlowerId == flowerId);
        }
    }
}
