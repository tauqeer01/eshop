using System;
using API.Extentions;
using Core.Dtos;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Specifications;
using Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(IUnitOfWork unit, ICartService cartService) : BaseApiController
{

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
    {
        var email = User.GetEmail();
        var cart = await cartService.GetCartAsync(orderDto.CartId);
        if (cart is null) return BadRequest("cart not found");
        if (cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");
        var items = new List<OrderItem>();
        foreach( var item in cart.Items){
            var productItem = await unit.BaseRepo<Product>().GetByIdAsync(item.ProductId);
            if (productItem is null) return BadRequest("Problem with order");
            var itemOrdered = new ProductItemOrdered
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };
            items.Add(orderItem);
        }

        var deliveryMethod = await unit.BaseRepo<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);
        if(deliveryMethod is null) return BadRequest("No delovery method selected");
        var subtotal = items.Sum(item => item.Price * item.Quantity);
        var order = new Order
        {
            BuyerEmail = email,
            DeliveryMethod = deliveryMethod,
            OrderItems = items,
            ShippingAddress=orderDto.ShippingAddress,
            SubTotal = subtotal,
            Status = OrderStatus.Pending,
            PaymentIntentId = cart.PaymentIntentId,
            PaymentSummary=orderDto.PaymentSummary,

        };
        unit.BaseRepo<Order>().Add(order);
        if(await unit.Complete()){
            return order;
        }
        return BadRequest("Problem creating order");
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser(){
        var spec = new OrderSpecifications(User.GetEmail());
        var orders = await unit.BaseRepo<Order>().ListAsync(spec);
        var ordersToReturn = orders.Select(order => order.ToDto()).ToList();
        return Ok(ordersToReturn);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id){
        var spec = new OrderSpecifications(User.GetEmail(),id );
        var order = await unit.BaseRepo<Order>().GetEntityWithSpec(spec);
        if(order == null) return NotFound();
        return order.ToDto();
    }
}
