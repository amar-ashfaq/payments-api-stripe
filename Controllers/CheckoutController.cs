using Microsoft.AspNetCore.Mvc;
using Payments.Entities;
using Stripe.Checkout;

namespace Payments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateCheckoutSession([FromBody] CheckoutFormEntity checkoutEntity)
        {
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
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Ok( new { checkoutUrl = session.Url });
        }
    }
}
