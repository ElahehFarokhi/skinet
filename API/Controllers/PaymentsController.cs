using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController(IPaymentService paymentService,
        IUnitOfWork uow, ILogger<PaymentsController> logger) : BaseApiController
    {
        private readonly string _whSecret = "";

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart is null) return BadRequest("Problem with your cart.");
            return Ok(cart);
        }

        [Authorize]
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await uow.Repository<DeliveryMethod>().ListAllAsync());
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = ConstructStripeEvent(json);
                if (stripeEvent.Data.Object is not PaymentIntent intent)
                {
                    return BadRequest("Invalid event data.");
                }
                await HandlePaymentIntentSucceeded(intent);
                return Ok();
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook error.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Stripe webhook error.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occured.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occured.");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            if (intent.Status == "succeeded")
            {
                var spec = new OrderSpecification(intent.Id, true);

                var order = await uow.Repository<Order>().GetEntityWithSpec(spec)
                    ?? throw new Exception("Order not found");

                var orderTotalInCents = (long)Math.Round(order.GetTotal() * 100,
                    MidpointRounding.AwayFromZero);

                if (orderTotalInCents != intent.Amount)
                {
                    order.Status = OrderStatus.PaymentMismatch;
                }
                else
                {
                    order.Status = OrderStatus.PaymentReceived;
                }

                await uow.Complete();
            }
        }

        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
                    _whSecret);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct stripe event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}
