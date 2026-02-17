using Microsoft.AspNetCore.Mvc;
using Payments.Entities;
using Stripe.Checkout;

namespace Payments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CheckoutController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutFormEntity checkoutEntity)
        {
            var payment = new Payment
            {
                PaymentStatus = PaymentStatus.Pending,
                Amount = 2500
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = checkoutEntity.Price,
                        Quantity = checkoutEntity.Quantity
                    }
                },

                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",

                ClientReferenceId = payment.Id.ToString(),

                Metadata =
                {
                    ["paymentId"] = payment.Id.ToString()
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Ok( new { checkoutUrl = session.Url });
        }
    }
}
