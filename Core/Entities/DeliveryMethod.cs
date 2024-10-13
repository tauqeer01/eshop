using System;

namespace Core.Entities;

public class DeliveryMethod : BaseEntities
{
    public required string ShortName { get; set; }
    public required string DeliveryTime { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
}
