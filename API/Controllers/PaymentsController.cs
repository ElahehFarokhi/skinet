using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentsController(IPaymentService paymentService,
        IGenericRepository<DeliveryMethod> dmRepository):BaseApiController
    {
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
            return Ok(await dmRepository.ListAllAsync());
        }


    }
}
