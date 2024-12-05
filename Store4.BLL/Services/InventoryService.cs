using Store4.DAL.Interfaces;
using Store4.Shared;
using System.Globalization;

namespace Store4.BLL.Services
{
    public class InventoryService

    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IShopRepository _shopRepository;
        public InventoryService(IInventoryRepository inventoryRepository, IShopRepository shopRepository)
        {
            _inventoryRepository = inventoryRepository;

            _shopRepository = shopRepository;
        }
        public async Task UpdateInventoryAsync(string shopCode, List<Inventory> items)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || items == null || !items.Any())
            {
                throw new ArgumentException("Shop code and inventory items are required.");
            }

            await _inventoryRepository.UpdateInventoryAsync(shopCode, items);
        }
        public async Task<string> GetCheapestShopForProductAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name is required.");
            }

            var inventories = await _inventoryRepository.GetInventoryByProductAsync(productName);

            if (!inventories.Any())
            {
                return $"Product {productName} not found in any shop.";
            }

            var cheapestProduct = inventories.OrderBy(i => i.Price).First();

            return $"The cheapest shop for {productName} is {cheapestProduct.ShopCode} with price {cheapestProduct.Price.ToString(CultureInfo.InvariantCulture)}.";
        }
        public async Task<List<PurchaseOption>> GetAffordableItemsForBudgetAsync(decimal budget)
        {
            var affordableItems = await _inventoryRepository.GetAffordableItemsAsync(budget);

            if (affordableItems.Any())
            {
                return affordableItems;
            }

            return new List<PurchaseOption> { new PurchaseOption { ProductName = "No items found within the budget." } };
        }

        public async Task<List<Inventory>> GetInventoryByShopAsync(string shopCode)
        {
            if (string.IsNullOrWhiteSpace(shopCode))
            {
                throw new ArgumentException("Shop code is required.");
            }

            return await _inventoryRepository.GetInventoryByShopAsync(shopCode);
        }

        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            return await _inventoryRepository.GetAllInventoryAsync();
        }
        public async Task<decimal> PurchaseItemsAsync(string shopCode, List<Inventory> purchaseRequests)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || purchaseRequests == null || !purchaseRequests.Any())
            {
                throw new ArgumentException("Shop code and purchase requests are required.");
            }

            // Вызываем метод репозитория для выполнения операции
            return await _inventoryRepository.PurchaseItemsAsync(shopCode, purchaseRequests);
        }
        public async Task<Shop?> FindCheapestShopForProductAsync(PurchaseRequest purchaseRequest)
        {
            // Получаем все магазины и инвентарь
            var shops = await _shopRepository.GetAllAsync();
            var inventories = await _inventoryRepository.GetInventoriesAsync();

            string? cheapestShopCode = null;
            decimal cheapestPrice = decimal.MaxValue;

            // Проходим по всем магазинам
            foreach (var shop in shops)
            {
                var shopInventories = inventories.Where(i => i.ShopCode == shop.Code).ToList();

                var inventoryItem = shopInventories.FirstOrDefault(i => i.ProductName == purchaseRequest.ProductName);

                // Если товар найден в магазине и его количество достаточное
                if (inventoryItem != null && inventoryItem.Quantity >= purchaseRequest.Quantity)
                {
                    decimal totalCost = inventoryItem.Price * purchaseRequest.Quantity;

                    // Если это самый дешевый магазин
                    if (totalCost < cheapestPrice)
                    {
                        cheapestPrice = totalCost;
                        cheapestShopCode = shop.Code;
                    }
                }
            }

            // Возвращаем магазин с наименьшей ценой, если он найден
            return cheapestShopCode == null ? null : await _shopRepository.GetShopByCodeAsync(cheapestShopCode);
        }


    }
}
