using ProductService.Business.Interfaces;
using ProductService.Data.Repositories.Interfaces;
using ProductService.Models;

namespace ProductService.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _inventoryRepository.GetAllAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int id)
        {
            return await _inventoryRepository.GetByIdAsync(id);
        }

        public async Task<Inventory?> GetInventoryByProductIdAsync(int productId)
        {
            return await _inventoryRepository.GetByProductIdAsync(productId);
        }

        public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
        {
            return await _inventoryRepository.AddAsync(inventory);
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            await _inventoryRepository.UpdateAsync(inventory);
        }

        public async Task DeleteInventoryAsync(int id)
        {
            var inventory =
                await _inventoryRepository.GetByIdAsync(id);

            if (inventory != null)
            {
                await _inventoryRepository.DeleteAsync(id);
            }
        }
    }
}