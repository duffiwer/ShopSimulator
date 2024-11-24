using Microsoft.EntityFrameworkCore;
using Store4.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store4.DAL
{
    public class DatabaseInventoryRepository : IInventoryRepository
    {
        private readonly StoreSimulatorContext _context;

        public DatabaseInventoryRepository(StoreSimulatorContext context)
        {
            _context = context;
        }

        public async Task UpdateInventoryAsync(string shopCode, List<Inventory> items)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || items == null || !items.Any())
            {
                throw new ArgumentException("Shop code and inventory items are required.");
            }

            var existingItems = await _context.Inventories
                .Where(i => i.ShopCode == shopCode)
                .ToListAsync();

            foreach (var item in items)
            {
                var existingItem = existingItems.FirstOrDefault(i => i.ProductName == item.ProductName);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                }
                else
                {
                    _context.Inventories.Add(new Inventory
                    {
                        ShopCode = shopCode,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Inventory>> GetInventoryByShopAsync(string shopCode)
        {
            if (string.IsNullOrWhiteSpace(shopCode))
            {
                throw new ArgumentException("Shop code is required.");
            }

            return await _context.Inventories
                .Where(i => i.ShopCode == shopCode)
                .ToListAsync();
        }

        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<List<PurchaseOption>> GetAffordableItemsAsync(decimal budget)
        {
            var availableItems = new List<PurchaseOption>();

            var inventories = await _context.Inventories.ToListAsync();

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
            return await _context.Inventories.ToListAsync();
        }

        public async Task UpdateInventoryWithNewItemsAsync(string shopCode, List<Inventory> items)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || items == null || !items.Any())
            {
                throw new ArgumentException("Shop code and inventory items are required.");
            }

            var existingItems = await _context.Inventories
                .Where(i => i.ShopCode == shopCode)
                .ToListAsync();

            foreach (var item in items)
            {
                var existingItem = existingItems.FirstOrDefault(i => i.ProductName == item.ProductName);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                }
                else
                {
                    _context.Inventories.Add(new Inventory
                    {
                        ShopCode = shopCode,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Inventory>> GetInventoryByProductAsync(string productName)
        {
            return await _context.Inventories
                .Where(i => i.ProductName == productName)
                .ToListAsync();
        }

        public async Task<decimal> PurchaseItemsAsync(string shopCode, List<Inventory> purchaseRequests)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || purchaseRequests == null || !purchaseRequests.Any())
            {
                throw new ArgumentException("Shop code and purchase requests are required.");
            }

            var totalCost = 0m;

            foreach (var request in purchaseRequests)
            {
                var inventoryItem = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ShopCode == shopCode && i.ProductName == request.ProductName);

                if (inventoryItem == null)
                {
                    throw new InvalidOperationException($"Product '{request.ProductName}' not found in shop '{shopCode}'.");
                }

                if (inventoryItem.Quantity < request.Quantity)
                {
                    throw new InvalidOperationException($"Not enough '{request.ProductName}' in shop '{shopCode}'.");
                }

                inventoryItem.Quantity -= request.Quantity;
                totalCost += request.Quantity * inventoryItem.Price;
            }

            await _context.SaveChangesAsync();
            return totalCost;
        }

        public async Task<List<Inventory>> GetInventoryByProductsAsync(string shopCode, List<string> productNames)
        {
            return await _context.Inventories
                .Where(i => i.ShopCode == shopCode && productNames.Contains(i.ProductName))
                .ToListAsync();
        }
    }
}
