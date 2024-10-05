using System;
using Core.Entities;

namespace Infrastructure.Repository.Interface;

public interface ICartService
{
 Task<ShoppingCart?> GetCartAsync(string key);
  Task<ShoppingCart?> SetCartAsync(ShoppingCart cart);
  Task<bool> DeleteCartAsync(string key);
}
