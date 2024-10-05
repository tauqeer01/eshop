using Core.Entities;
using Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController(ICartService cartService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCart(string id)
        {
           var cart = await cartService.GetCartAsync(id);
            return Ok(cart ?? new ShoppingCart{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await cartService.SetCartAsync(cart);
            if(updatedCart == null) return BadRequest("Problem updating the cart");
            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
           var result= await cartService.DeleteCartAsync(id);
           if(!result) return BadRequest("Problem deleting the cart");
           return Ok();
        }
    }
}
