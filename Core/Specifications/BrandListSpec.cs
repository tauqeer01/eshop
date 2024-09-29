using System;
using Core.Entities;

namespace Core.Specifications;

public class BrandListSpec : BaseSpecifications<Product, string>
{
  public BrandListSpec()
  {
    AddSelect(x=>x.Brand);
    ApplyDistinct();
  }
}
