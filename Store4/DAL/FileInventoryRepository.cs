using Store4.Shared;
using System.Globalization;
using System.Text;

namespace Store4.DAL
{
    public class FileInventoryRepository : IInventoryRepository
    {
        private readonly string _filePath = "inventory.csv";
       

        public async Task UpdateInventoryAsync(string shopCode, List<Inventory> items)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || items == null || !items.Any())
            {
                throw new ArgumentException("Shop code and inventory items are required.");
            }

            var lines = File.Exists(_filePath)
                ? await File.ReadAllLinesAsync(_filePath)
                : Array.Empty<string>();

            var updatedLines = new List<string>();

            foreach (var line in lines)
            {
                var columns = line.Split(',');

                if (columns.Length == 4 && columns[0] == shopCode)
                {
                    var productName = columns[1];
                    var item = items.FirstOrDefault(i => i.ProductName == productName);

                    if (item != null)
                    {
                        updatedLines.Add($"{shopCode},{item.ProductName},{item.Price.ToString(CultureInfo.InvariantCulture)},{item.Quantity}");
                        items.Remove(item);
                    }
                    else
                    {
                        updatedLines.Add(line);
                    }
                }
                else
                {
                    updatedLines.Add(line);
                }
            }

            foreach (var item in items)
            {
                updatedLines.Add($"{shopCode},{item.ProductName},{item.Price.ToString(CultureInfo.InvariantCulture)},{item.Quantity}");
            }

            await File.WriteAllLinesAsync(_filePath, updatedLines);
        }

        

        public async Task<List<Inventory>> GetInventoryByShopAsync(string shopCode)
        {
            if (string.IsNullOrWhiteSpace(shopCode))
            {
                throw new ArgumentException("Shop code is required.");
            }

            var inventories = new List<Inventory>();

            if (File.Exists(_filePath))
            {
                var lines = await File.ReadAllLinesAsync(_filePath);

                foreach (var line in lines)
                {
                    var columns = line.Split(',');

                    if (columns.Length == 4 && columns[0] == shopCode)
                    {
                        var inventory = new Inventory
                        {
                            ShopCode = columns[0],
                            ProductName = columns[1],
                            Price = decimal.Parse(columns[2]),
                            Quantity = int.Parse(columns[3]),
                        };
                        inventories.Add(inventory);
                    }
                }
            }

            return inventories;
        }
        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            var inventories = new List<Inventory>();

            if (File.Exists(_filePath))
            {
                var lines = await File.ReadAllLinesAsync(_filePath);

                foreach (var line in lines)
                {
                    var columns = line.Split(',');

                    if (columns.Length == 4)
                    {
                        var inventory = new Inventory
                        {
                            ShopCode = columns[0],
                            ProductName = columns[1],
                            Price = decimal.Parse(columns[2], CultureInfo.InvariantCulture) , 
                            Quantity = int.Parse(columns[3]),
                        };
                        inventories.Add(inventory);
                    }
                }
            }

            return inventories;
        }
        public async Task<List<PurchaseOption>> GetAffordableItemsAsync(decimal budget)
        {
            var availableItems = new List<PurchaseOption>();

            var inventories = await GetAllInventoryAsync();

            foreach (var inventory in inventories)
            {
                if (inventory.Price <= budget)
                {
                    int maxQuantity = (int)(budget / inventory.Price);
                    availableItems.Add(new PurchaseOption
                    {
                        ProductName = inventory.ProductName,
                        Price = inventory.Price,
                        MaxQuantity = maxQuantity,
                        ShopCode = inventory.ShopCode
                    });
                }
            }

            return availableItems;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesAsync()
        {
            var inventories = new List<Inventory>();

            if (File.Exists(_filePath))
            {
                var lines = await File.ReadAllLinesAsync(_filePath);
                foreach (var line in lines)
                {
                    var columns = line.Split(',');
                    if (columns.Length == 4)
                    {
                        inventories.Add(new Inventory
                        {
                            ShopCode = columns[0],
                            ProductName = columns[1],
                            Price = decimal.Parse(columns[2], CultureInfo.InvariantCulture),
                            Quantity = int.Parse(columns[3])
                        });
                    }
                }
            }

            return inventories;
        }
        public async Task UpdateInventoryWithNewItemsAsync(string shopCode, List<Inventory> items)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || items == null || !items.Any())
            {
                throw new ArgumentException("Shop code and inventory items are required.");
            }

            var lines = File.Exists(_filePath) ? await File.ReadAllLinesAsync(_filePath) : Array.Empty<string>();
            var updatedLines = new List<string>();

            foreach (var line in lines)
            {
                var columns = line.Split(',');

                if (columns.Length == 4 && columns[0] == shopCode)
                {
                    var productName = columns[1];
                    var item = items.FirstOrDefault(i => i.ProductName == productName);

                    if (item != null)
                    {
                        updatedLines.Add($"{shopCode},{item.ProductName},{item.Price.ToString(CultureInfo.InvariantCulture)},{item.Quantity}");
                        items.Remove(item);
                    }
                    else
                    {
                        updatedLines.Add(line);
                    }
                }
                else
                {
                    updatedLines.Add(line);
                }
            }

            foreach (var item in items)
            {
                updatedLines.Add($"{shopCode},{item.ProductName},{item.Price.ToString(CultureInfo.InvariantCulture)},{item.Quantity}");
            }

            await File.WriteAllLinesAsync(_filePath, updatedLines);
        }
        public async Task<List<Inventory>> GetInventoryByProductAsync(string productName)
        {
            var inventories = new List<Inventory>();

            if (File.Exists(_filePath))
            {
                var lines = await File.ReadAllLinesAsync(_filePath);

                foreach (var line in lines)
                {
                    var columns = line.Split(',');

                    if (columns.Length == 4 && columns[1] == productName)
                    {
                        try
                        {
                            var inventory = new Inventory
                            {
                                ShopCode = columns[0],
                                ProductName = columns[1],
                                Price = decimal.Parse(columns[2], CultureInfo.InvariantCulture),
                                Quantity = int.Parse(columns[3])
                            };
                            inventories.Add(inventory);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing line: {line}. Error: {ex.Message}");
                        }
                    }
                }
            }
            return inventories;
        }

        public async Task<decimal> PurchaseItemsAsync(string shopCode, List<Inventory> purchaseRequests)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || purchaseRequests == null || !purchaseRequests.Any())
            {
                throw new ArgumentException("Shop code and purchase requests are required.");
            }

            var lines = File.Exists(_filePath) ? await File.ReadAllLinesAsync(_filePath) : Array.Empty<string>();
            var updatedLines = new List<string>();
            var totalCost = 0m; 
            var processedProducts = new HashSet<string>();

            foreach (var request in purchaseRequests)
            {
                var line = lines.FirstOrDefault(l =>
                {
                    var columns = l.Split(',');
                    return columns.Length == 4 && columns[0] == shopCode && columns[1] == request.ProductName;
                });

                if (line == null)
                {
                    throw new InvalidOperationException($"Product '{request.ProductName}' not found in shop '{shopCode}'.");
                }

                var columns = line.Split(',');
                var currentQuantity = int.Parse(columns[3]);
                var price = decimal.Parse(columns[2], CultureInfo.InvariantCulture);
                if (currentQuantity < request.Quantity)
                {
                    throw new InvalidOperationException($"Not enough '{request.ProductName}' in shop '{shopCode}'.");
                }

                var remainingQuantity = currentQuantity - request.Quantity;

                totalCost += request.Quantity * price;

                updatedLines.Add($"{shopCode},{request.ProductName},{price.ToString(CultureInfo.InvariantCulture)},{remainingQuantity}");

                processedProducts.Add($"{shopCode},{request.ProductName}");
            }

            updatedLines.AddRange(lines.Where(line =>
            {
                var columns = line.Split(',');
                return columns.Length != 4 || columns[0] != shopCode || !processedProducts.Contains($"{columns[0]},{columns[1]}");
            }));

            await File.WriteAllLinesAsync(_filePath, updatedLines);

            return totalCost;
        }
        public async Task<List<Inventory>> GetInventoryByProductsAsync(string shopCode, List<string> productNames)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || productNames == null || !productNames.Any())
            {
                throw new ArgumentException("Shop code and product names are required.");
            }

            var lines = File.Exists(_filePath) ? await File.ReadAllLinesAsync(_filePath) : Array.Empty<string>();
            var result = new List<Inventory>();

            foreach (var line in lines)
            {
                var columns = line.Split(',');

                if (columns.Length == 4 && columns[0] == shopCode && productNames.Contains(columns[1]))
                {
                    result.Add(new Inventory
                    {
                        ShopCode = columns[0],
                        ProductName = columns[1],
                        Price = decimal.Parse(columns[2], CultureInfo.InvariantCulture),
                        Quantity = int.Parse(columns[3])
                    });
                }
            }

            return result;
        }




    }

}
