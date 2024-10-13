using Core.Entities;
using Core.Interface;
using Infrastructure.Migrations;
using Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class PaymentController(IPaymentService paymentService, IBaseRepo<DeliveryMethod> dmRepo) : BaseApiController
    {
        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null) return BadRequest("Problem with your cart");
            return Ok(cart);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliverMethods()
        {
            return Ok(await dmRepo.GetAllAsync());
        }
    }
}
