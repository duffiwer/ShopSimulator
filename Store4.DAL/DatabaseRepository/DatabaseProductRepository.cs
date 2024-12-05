
using Store4.Shared;
using Microsoft.EntityFrameworkCore;
using Store4.DAL.Interfaces;
using Store4.DAL.Context;


namespace Store4.DAL.DatabaseRepository
{
    public class DatabaseProductRepository : IProductRepository
    {
        private readonly StoreSimulatorContext _context;

        public DatabaseProductRepository(StoreSimulatorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product must have a valid name.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}


