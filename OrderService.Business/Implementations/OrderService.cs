using Azure.Core;
using OrderService.Business.Interfaces;
using OrderService.Business.Services;
using OrderService.Data.Repositories.Interfaces;
using OrderService.Models;

namespace OrderService.Business.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ProductServiceClient _productServiceClient;
        private readonly PaymentServiceClient _paymentServiceClient;

        public OrderService(
            IOrderRepository orderRepository,
            ProductServiceClient productServiceClient,
            PaymentServiceClient paymentServiceClient)
        {
            _orderRepository = orderRepository;
            _productServiceClient = productServiceClient;
            _paymentServiceClient = paymentServiceClient;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = 0;

            foreach (var item in order.Items)
            {
                var product =
                    await _productServiceClient
                        .GetProductByIdAsync(item.ProductId);

                if (product == null)
                {
                    throw new KeyNotFoundException(
                        $"Product with ID {item.ProductId} was not found.");
                }

                var inventory =
                    await _productServiceClient
                        .GetInventoryByProductIdAsync(item.ProductId);

                if (inventory == null)
                {
                    throw new KeyNotFoundException(
                        $"Inventory for product {item.ProductId} was not found.");
                }

                if (inventory.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {item.ProductId}. " +
                        $"Available: {inventory.Quantity}");
                }

                item.UnitPrice = product.Price;

                item.TotalPrice =
                    item.UnitPrice * item.Quantity;

                order.TotalAmount += item.TotalPrice;
            }

            // Save the order first
            var createdOrder =
                await _orderRepository.AddAsync(order);

            // Create payment
            var payment =
                await _paymentServiceClient.CreatePaymentAsync(
                    createdOrder.OrderId,
                    createdOrder.TotalAmount,
                    createdOrder.PaymentMethod);

            if (payment == null)
            {
                throw new InvalidOperationException(
                    "Payment failed.");
            }
            createdOrder.Status = "Paid";

            await _orderRepository.UpdateAsync(createdOrder);

            return createdOrder;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order =
                await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException(
                    $"Order with ID {id} was not found.");
            }

            await _orderRepository.DeleteAsync(order);
        }
    }
}