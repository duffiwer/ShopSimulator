using Store4.Shared;

namespace Store4.DAL
{
    public interface IShopRepository
    {
        Task<Shop?> GetShopByCodeAsync(string code);

        Task<IEnumerable<Shop>> GetAllAsync(); 
        Task AddShopAsync(Shop shop); 
    }
}
