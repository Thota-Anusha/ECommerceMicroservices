using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using OrderService.Models;

namespace OrderService.Business.Services
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(
                configuration["Services:ProductService"]
                ?? "https://localhost:7241/");
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>(
                $"api/Product/{productId}");
        }

        public async Task<ProductInventoryDto?> GetInventoryByProductIdAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ProductInventoryDto>(
                $"api/Inventory/product/{productId}");
        }
    }
}