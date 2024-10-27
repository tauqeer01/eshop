using System;

namespace Core.Entities.OrderAggregate;

public class OrderItem : BaseEntities
{
    public ProductItemOrdered ItemOrdered { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }    
}
