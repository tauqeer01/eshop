using System;
using System.Linq.Expressions;

namespace Core.Interface;

public interface ISpecifications<T>
{
  Expression<Func<T, bool>>? Criteria { get; }
  Expression<Func<T, object>>? OrderBy { get; }
  Expression<Func<T, object>>? OrderByDescending { get; }
  bool IsDistinct { get; }
  //int Take { get; }
  //int Skip { get; }
  //bool IsPagingEnabled { get; }

}
public interface ISpecifications<T, TResult> : ISpecifications<T>
{
  Expression<Func<T, TResult>>? Select { get; }
}
