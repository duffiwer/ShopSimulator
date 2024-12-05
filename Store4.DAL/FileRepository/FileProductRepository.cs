using Store4.DAL.Interfaces;
using Store4.Shared;
using System.Globalization;
using System.Text;

namespace Store4.DAL.FileRepository
{
    public class FileProductRepository : IProductRepository
    {
        private readonly string _filePath = "Data/products.csv";

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();

            using (var reader = new StreamReader(_filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        products.Add(new Product { Name = line.Trim() });
                    }
                }
            }

            return products;
        }
        public async Task AddProductAsync(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product must have a valid name.");
            }

            var line = product.Name;

            await File.AppendAllTextAsync(_filePath, line + Environment.NewLine);
        }

    }
}