using System;
using System.Linq.Expressions;

namespace Core.Interface;

public interface ISpecifications<T>
{
  Expression<Func<T, bool>>? Criteria { get; }
  Expression<Func<T, object>>? OrderBy { get; }
  Expression<Func<T, object>>? OrderByDescending { get; }
  List<Expression<Func<T,object>>> Includes { get; }
  List<string> IncludeStrings { get; } // For ThenIncludes 
  bool IsDistinct { get; }
  int Take { get; }
  int Skip { get; }
  bool IsPagingEnabled { get; }
  IQueryable<T> ApplyCriteria(IQueryable<T> query);
}
public interface ISpecifications<T, TResult> : ISpecifications<T>
{
  Expression<Func<T, TResult>>? Select { get; }
}
