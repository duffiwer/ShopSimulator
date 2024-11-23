using Store4.DAL;
using Store4.Shared;

namespace Store4.BLL
{
    public class ShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }
        public async Task AddShopAsync(Shop shop)
        {
            if (shop == null)
            {
                throw new ArgumentNullException(nameof(shop));
            }

            await _shopRepository.AddShopAsync(shop);
        }
        public async Task<Shop?> GetShopByIdAsync(string id)
        {
            return await _shopRepository.GetShopByCodeAsync(id);
        }


    }
}
