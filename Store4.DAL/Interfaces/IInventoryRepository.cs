using Store4.Shared;

namespace Store4.DAL.Interfaces
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetInventoryByShopAsync(string shopCode);
        Task UpdateInventoryAsync(string shopCode, List<Inventory> items);
        Task<List<Inventory>> GetAllInventoryAsync();
        Task<List<Inventory>> GetInventoryByProductAsync(string productName);
        Task<List<PurchaseOption>> GetAffordableItemsAsync(decimal budget);

        Task<decimal> PurchaseItemsAsync(string shopCode, List<Inventory> purchaseRequests);
        Task<List<Inventory>> GetInventoryByProductsAsync(string shopCode, List<string> productNames);
        Task<IEnumerable<Inventory>> GetInventoriesAsync();
    }
}
