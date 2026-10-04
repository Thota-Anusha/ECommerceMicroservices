using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Business.Interfaces;
using PaymentService.Models;
using PaymentService.Models.DTOs;
using System.Security.Claims;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments =
                await _paymentService.GetAllPaymentsAsync();

            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment =
                await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(
            CreatePaymentRequest request)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(
                    "User ID not found in token.");
            }

            if (!int.TryParse(
                    userIdClaim.Value,
                    out int userId))
            {
                return Unauthorized(
                    "Invalid User ID in token.");
            }

            var payment = new Payment
            {
                OrderId = request.OrderId,
                UserId = userId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod
            };

            var createdPayment =
                await _paymentService
                    .CreatePaymentAsync(payment);

            return Ok(createdPayment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(
            int id,
            Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            await _paymentService
                .UpdatePaymentAsync(payment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentService
                .DeletePaymentAsync(id);

            return NoContent();
        }
    }
}