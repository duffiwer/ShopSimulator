using Store4.DAL.Interfaces;
using Store4.Shared;

namespace Store4.BLL.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductService(IProductRepository productRepository, IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        public async Task AddProductAsync(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }

            await _productRepository.AddProductAsync(product);
        }


    }
}
