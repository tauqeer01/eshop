using System;
using System.Linq.Expressions;
using Core.Interface;

namespace Core.Specifications;

public class BaseSpecifications<T>(Expression<Func<T, bool>>? criteria) : ISpecifications<T>
{
    protected BaseSpecifications() : this(null) { }
    public Expression<Func<T, bool>>? Criteria => criteria;
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
