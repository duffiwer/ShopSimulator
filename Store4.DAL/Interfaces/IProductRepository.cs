using Store4.Shared;

namespace Store4.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddProductAsync(Product product);
    }
}
