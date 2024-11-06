using System;
using Core.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderSpecifications : BaseSpecifications<Order>
{
  public OrderSpecifications(string email) : base(o => o.BuyerEmail == email)
  {
    AddInclude(o => o.OrderItems);
    AddInclude(o => o.DeliveryMethod);
    AddOrderByDescending(o => o.OrderDate);
  }

  public OrderSpecifications(string email, int id): base(o => o.Id == id && o.BuyerEmail == email)
  {
    AddInclude("OrderItems");
    AddInclude("DeliveryMethod");
  }
}
