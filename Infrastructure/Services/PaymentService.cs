using Core.Entities;
using Core.Interfaces;
using Stripe;
using Microsoft.Extensions.Configuration;

using ProductEntity = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentService(IConfiguration config, ICartService cartService, IGenericRepository<ProductEntity> productRepo, IGenericRepository<DeliveryMethod> dmRepository) : IPaymentService
    {
        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cardId)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
            var cart = await cartService.GetCartAsync(cardId);
            if (cart is null) return null;
            var shippingPrice = 0m;

            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await dmRepository.GetByIdAsync((int)cart.DeliveryMethodId);
                if (deliveryMethod is null) return null;
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.Items)
            {
                var productItem = await productRepo.GetByIdAsync(item.ProductId);
                if (productItem is null) return null;
                item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent? intent = null;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)
                    + (long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else 
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)
                    + (long)shippingPrice * 100)
                };
                intent = await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            await cartService.SetCartAsync(cart);

            return cart;

        }
    }
}
