using Store4.DAL.Interfaces;
using Store4.Shared;
using System.Text;

namespace Store4.DAL.FileRepository
{
    public class FileShopRepository : IShopRepository
    {
        private readonly string _filePath = "Data/shops.csv";

        public async Task<IEnumerable<Shop>> GetAllAsync()
        {
            var shops = new List<Shop>();

            using (var reader = new StreamReader(_filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var columns = line.Split(',');

                    if (columns.Length >= 3)
                    {
                        shops.Add(new Shop
                        {
                            Code = columns[0],
                            Name = columns[1],
                            Address = columns[2]
                        });
                    }
                }
            }

            return shops;
        }
        public async Task<Shop?> GetShopByCodeAsync(string code)
        {
            var shops = await GetAllAsync();
            return shops.FirstOrDefault(s => s.Code == code);
        }
        public async Task AddShopAsync(Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException(nameof(shop));

            var line = $"{shop.Code},{shop.Name},{shop.Address}";

            using (var writer = new StreamWriter(_filePath, append: true, Encoding.UTF8))
            {
                await writer.WriteLineAsync(line);
            }
        }

    }
}
