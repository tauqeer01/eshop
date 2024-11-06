using System;
using Core.Dtos;
using Core.Entities.OrderAggregate;

namespace API.Extentions;

public static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            BuyerEmail = order.BuyerEmail,
            OrderDate = order.OrderDate,
            ShippingAddress = order.ShippingAddress,
            DeliveryMethod = order.DeliveryMethod.Description,
            SubTotal = order.SubTotal,
            Total = order.GetOrderTotal(),
            Status = order.Status.ToString(),
            PaymentIntentId = order.PaymentIntentId,
            OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
            ShippingPrice = order.DeliveryMethod.Price,
            PaymentSummary = order.PaymentSummary
        };
    }

    public static OrderItemsDto ToDto(this OrderItem orderItem)
    {
        return new OrderItemsDto
        {
            ProductId = orderItem.ItemOrdered.ProductId,
            ProductName = orderItem.ItemOrdered.ProductName,
            PictureUrl = orderItem.ItemOrdered.PictureUrl,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity
        };
    }
}
