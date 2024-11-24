using Microsoft.EntityFrameworkCore;
using Store4.Shared;

namespace Store4.DAL
{
    public class DatabaseShopRepository : IShopRepository
    {
        private readonly StoreSimulatorContext _context;

        public DatabaseShopRepository(StoreSimulatorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shop>> GetAllAsync()
        {
            return await _context.Shops.ToListAsync();
        }

        public async Task<Shop?> GetShopByCodeAsync(string code)
        {
            return await _context.Shops.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task AddShopAsync(Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException(nameof(shop));

            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();
        }
    }

}
