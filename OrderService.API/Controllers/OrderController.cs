using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Business.Interfaces;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            // Get UserId from JWT token
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid User ID in token.");
            }

            // Validate payment method
            var allowedPaymentMethods = new[]
            {
                "Card",
                "UPI",
                "CashOnDelivery"
            };

            if (!allowedPaymentMethods.Contains(
                    request.PaymentMethod,
                    StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(
                    "Invalid payment method. Use Card, UPI, or CashOnDelivery.");
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,

                Items = request.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            try
            {
                var createdOrder =
                    await _orderService.CreateOrderAsync(order);

                return Ok(createdOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            int id,
            Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            await _orderService.UpdateOrderAsync(order);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);

            return NoContent();
        }
    }
}