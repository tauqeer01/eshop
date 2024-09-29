using System;
using Core.Entities;

namespace Core.Specifications;

public class TypesListSpecification : BaseSpecifications<Product, string>
{
    public TypesListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
