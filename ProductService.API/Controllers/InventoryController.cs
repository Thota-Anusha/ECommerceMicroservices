using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Business.Interfaces;
using ProductService.Models;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventory =
                await _inventoryService.GetAllInventoryAsync();

            return Ok(inventory);
        }

        // GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            var inventory =
                await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // GET: api/Inventory/product/{productId}
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInventoryByProductId(int productId)
        {
            var inventory =
                await _inventoryService.GetInventoryByProductIdAsync(productId);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // POST: api/Inventory
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateInventory(
            Inventory inventory)
        {
            var createdInventory =
                await _inventoryService.CreateInventoryAsync(inventory);

            return Ok(createdInventory);
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInventory(
            int id,
            Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return BadRequest();
            }

            await _inventoryService.UpdateInventoryAsync(inventory);

            return NoContent();
        }

        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);

            return NoContent();
        }
    }
}