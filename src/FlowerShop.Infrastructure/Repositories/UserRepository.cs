using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> GetFirstUserIdAsync()
        {
            return await _dbContext.Users.Select(x => x.UserId).FirstOrDefaultAsync();
        }
    }
}
