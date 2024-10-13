using System;
using Core.Entities;

namespace Core.Interface;

public interface IPaymentService
{
    Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cartId);
}
