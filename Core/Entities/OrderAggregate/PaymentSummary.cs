using System;

namespace Core.Entities.OrderAggregate;

public class PaymentSummary
{
  public int Last4 { get; set; }
  public string Brand { get; set; }

  public DateTime ExpMonth { get; set; }
  public int ExpYear { get; set; }
 

}
