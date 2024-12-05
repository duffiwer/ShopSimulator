using Microsoft.AspNetCore.Mvc;
using Store4.BLL.Services;
using Store4.Shared;

namespace Store4.Pages
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopSimulatorController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ShopService _shopService;
        private readonly InventoryService _inventoryService;
       public ShopSimulatorController(ProductService productService, ShopService shopService, InventoryService inventoryService)
        {
           _productService = productService;
            _shopService = shopService;
            _inventoryService = inventoryService;
        }
     
        [HttpPost("add-inventory")]
        public async Task<IActionResult> UpdateInventory([FromQuery] string shopCode, [FromBody] List<Inventory> items)
        {
            if (string.IsNullOrWhiteSpace(shopCode) || items == null || !items.Any())
            {
                return BadRequest("Shop code and inventory items are required.");
            }

            await _inventoryService.UpdateInventoryAsync(shopCode, items);
            return Ok("Inventory updated successfully.");
        }
        [HttpPost("add-shop")]
        public async Task<IActionResult> AddShop([FromBody] Shop shop)
        {
            if (shop == null || string.IsNullOrEmpty(shop.Code))
            {
                return BadRequest("Invalid shop data.");
            }

            await _shopService.AddShopAsync(shop);
            return Ok("Shop added successfully.");
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("Product name is required.");
            }

            await _productService.AddProductAsync(product);
            return Ok("Product added successfully.");
        }
        [HttpGet("cheapest-shop")]
        public async Task<IActionResult> GetCheapestShopForProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Product name is required.");
            }

            var cheapestShop = await _inventoryService.GetCheapestShopForProductAsync(productName);
            return Ok(cheapestShop);
        }
        /*
        [HttpGet("inventory")]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventoryList = await _inventoryService.GetAllInventoryAsync();

            if (inventoryList == null || !inventoryList.Any())
            {
                return NotFound("No inventory found.");
            }

            return Ok(inventoryList);
        }
        */
        [HttpGet("affordable-items")]
        public async Task<IActionResult> GetAffordableItems(decimal budget)
        {
            if (budget <= 0)
            {
                return BadRequest("Budget must be greater than 0.");
            }

            var affordableItems = await _inventoryService.GetAffordableItemsForBudgetAsync(budget);

            return Ok(affordableItems);

        }
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseItems([FromBody] PurchaseRequestModel requestModel)
        {
            try
            {
                if (requestModel == null || string.IsNullOrWhiteSpace(requestModel.ShopCode) ||
                    requestModel.PurchaseRequests == null || !requestModel.PurchaseRequests.Any())
                {
                    return BadRequest("Invalid purchase request.");
                }

                var purchaseRequests = requestModel.PurchaseRequests.Select(pr => new Inventory
                {
                    ShopCode = requestModel.ShopCode,
                    ProductName = pr.ProductName,
                    Quantity = pr.Quantity
                }).ToList();

                var totalCost = await _inventoryService.PurchaseItemsAsync(requestModel.ShopCode, purchaseRequests);

                return Ok(new { TotalCost = totalCost });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("FindCheapestShop")]
        public async Task<IActionResult> FindCheapestShop([FromBody] PurchaseRequest purchaseRequest)
        {
            if (purchaseRequest == null || string.IsNullOrWhiteSpace(purchaseRequest.ProductName) || purchaseRequest.Quantity <= 0)
            {
                return BadRequest("Valid product name and quantity are required.");
            }

            var shop = await _inventoryService.FindCheapestShopForProductAsync(purchaseRequest);

            if (shop == null)
            {
                return NotFound("No shop can fulfill the request.");
            }

            return Ok(new
            {
                shop.Code,
                shop.Name,
                shop.Address
            });
        }

    }
}