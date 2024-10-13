using System;
using Core.Entities;
using Core.Interface;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Repository;

public class PaymentService(
     IConfiguration config,
     ICartService cartService,
     IBaseRepo<Core.Entities.Product> productRepo,
     IBaseRepo<DeliveryMethod> deliveryMethodRepo) : IPaymentService
{
    public async Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        var cart = await cartService.GetCartAsync(cartId);
        if (cart == null) return null!;

        var shippingPrice = 0m;

        if (cart.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await deliveryMethodRepo.GetByIdAsync((int)cart.DeliveryMethodId);
            if (deliveryMethod == null) return null!;
            shippingPrice = deliveryMethod.Price;
        }

        foreach (var item in cart.Items)
        {
            var productItem = await productRepo.GetByIdAsync(item.ProductId);
            if (productItem == null) return null!;
            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }

        }

        var service = new PaymentIntentService();
        PaymentIntent? intent = null;
        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {

                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            intent = await service.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                + (long)shippingPrice * 100,
            };
            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }

        await cartService.SetCartAsync(cart);

        return cart;

    }
}