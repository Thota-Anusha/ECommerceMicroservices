using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OrderService.Models;

namespace OrderService.Business.Services
{
    public class PaymentServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentServiceClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            _httpClient.BaseAddress = new Uri(
                configuration["Services:PaymentService"]
                ?? "https://localhost:7034/");
        }

        public async Task<PaymentDto?> CreatePaymentAsync(
            int orderId,
            decimal amount,
            string paymentMethod)
        {
            var token =
                _httpContextAccessor.HttpContext?
                    .Request.Headers.Authorization
                    .ToString()
                    .Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            var request = new
            {
                orderId,
                amount,
                paymentMethod
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/Payment",
                request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<PaymentDto>();
        }
    }
}